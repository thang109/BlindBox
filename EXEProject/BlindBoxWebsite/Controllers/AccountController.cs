﻿using BlindBoxWebsite.DTO.AccountDTOs;
using BlindBoxWebsite.Interfaces;
using BlindBoxWebsite.Models;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Org.BouncyCastle.Crypto.Generators;

namespace BlindBoxWebsite.Controllers
{
    public class AccountController : Controller
    {
        private readonly BlindBoxContext _context;
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly ISendMailService _sendMailService;
        private readonly IDistributedCache _cache;

        public AccountController(BlindBoxContext context, IConfiguration configuration, IUserRepository userRepository, ISendMailService sendMailService, IDistributedCache cache)
        {
            _context = context;
            _configuration = configuration;
            _userRepository = userRepository;
            _sendMailService = sendMailService;
            _cache = cache;
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpRequest signUpRequest)
        {
            if (!ModelState.IsValid)
            {
                return View(signUpRequest);
            }

            var existingUser = _userRepository.GetByEmail(signUpRequest.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "Email is already in use.");
                return View(signUpRequest);
            }

            var user = new User
            {
                Username = signUpRequest.UserName,
                Email = signUpRequest.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(signUpRequest.Password),
                ResetToken = Guid.NewGuid().ToString(),
                IsEmailConfirmed = false
            };

            await _context.AddAsync(user);
            await _context.SaveChangesAsync();

            var confirmationLink = Url.Action("ConfirmEmail", "Account", new { userId = user.UserId, message = "Email confirmed successfully, please log in." }, Request.Scheme);

            var emailContent = new DTO.MailDTOs.MailContent
            {
                To = user.Email,
                Subject = "Email Confirmation",
                Body = $"<h1>Confirm Your Email</h1><p>Please click the link below to confirm your email address: <a href='{confirmationLink}'>Confirm Email</a></p>"
            };
            await _sendMailService.SendMail(emailContent);

            TempData["ConfirmMessage"] = "Please check your email to confirm your account.";

            return RedirectToAction("SignIn", "Account");
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            if (user.IsEmailConfirmed)
            {
                return RedirectToAction("SignIn", new { message = "Your email is already confirmed." });
            }

            user.IsEmailConfirmed = true;
            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();


            return RedirectToAction("SignIn");
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInRequest signInRequest)
        {
            if (!ModelState.IsValid)
            {
                return View(signInRequest);
            }

            var user = _userRepository.GetByEmail(signInRequest.UserName);

            if (user == null || !BCrypt.Net.BCrypt.Verify(signInRequest.Password, user.Password))
            {
                ModelState.AddModelError("Password", "Invalid email or password.");
                return View(signInRequest);
            }

            if (!user.IsEmailConfirmed)
            {
                ViewData["ErrorMessage"] = "Your account is not confirmed. Please check your email to confirm your account.";
                return View(signInRequest);
            }
            HttpContext.Session.SetString("UserId", user.UserId.ToString());
            HttpContext.Session.SetString("UserName", user.Username);

            TempData["LogInSuccess"] = "Welcome to my GBOX shop"; 

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["LogoutSuccess"] = "Thank you and see you soon";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest changePasswordRequest)
        {
            if (!ModelState.IsValid)
            {
                var userId = HttpContext.Session.GetString("UserId");
                if (userId != null)
                {
                    var userDb = await _userRepository.GetByIdAsync(int.Parse(userId));
                    changePasswordRequest.UserName = userDb?.Username;
                }
                return View(changePasswordRequest);
            }
            
            var userIdSession = HttpContext.Session.GetString("UserId");
            if(userIdSession == null)
            {
                return RedirectToAction("SignIn");
            }

            var user = await _userRepository.GetByIdAsync(int.Parse(userIdSession));
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "User not found.");
                return View(changePasswordRequest);
            }

            if (!BCrypt.Net.BCrypt.Verify(changePasswordRequest.OldPassword, user.Password))
            {
                ModelState.AddModelError(string.Empty, "Current password is incorrect.");
                return View(changePasswordRequest);
            }

            if (changePasswordRequest.NewPassword != changePasswordRequest.ConfirmPassword)
            {
                ModelState.AddModelError(string.Empty, "New password and confirmation do not match.");
                return View(changePasswordRequest);
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(changePasswordRequest.NewPassword);

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(DTO.AccountDTOs.ForgotPasswordRequest forgotPasswordRequest)
        {
            if(!ModelState.IsValid)
            {
                return View(forgotPasswordRequest);
            }

            var user = _userRepository.GetByEmail(forgotPasswordRequest.Email);
            if (user == null)
            {
                ModelState.AddModelError("Email", "Email not found.");
                return View(forgotPasswordRequest);
            }

            var resetToken = Guid.NewGuid().ToString();
            user.ResetToken = resetToken;
            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            var resetPasswordLink = Url.Action("ResetPassword", "Account", new { token = resetToken, email = user.Email }, Request.Scheme);

            var emailContent = new DTO.MailDTOs.MailContent
            {
                To = user.Email,
                Subject = "Reset Your Password",
                Body = $"<h1>Reset Your Password</h1><p>Click the link below to reset your password: <a href='{resetPasswordLink}'>Reset Password</a></p>"
            };

            await _sendMailService.SendMail(emailContent);
            TempData["ResetMessage"] = "Please check your email for a link to reset your password.";

            return RedirectToAction("ForgotPassword", "Account");
        }

        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            var model = new DTO.AccountDTOs.ResetPasswordRequest
            {
                Token = token,
                Email = email
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(DTO.AccountDTOs.ResetPasswordRequest resetPasswordRequest)
        {
            if (!ModelState.IsValid)
            {
                return View(resetPasswordRequest);
            }

            var user = _userRepository.GetByEmail(resetPasswordRequest.Email);
            if (user == null || user.ResetToken != resetPasswordRequest.Token)
            {
                ModelState.AddModelError("NewPassword", "Token is already used");
                return View(resetPasswordRequest);
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(resetPasswordRequest.NewPassword);
            user.ResetToken = null; 
            user.UpdatedAt = DateTime.Now;

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            TempData["ResetSuccess"] = "Password reset successfully, please sign in again";

            return RedirectToAction("SignIn", "Account");
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var userId = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("SignIn");
            }

            var user = await _userRepository.GetByIdAsync(int.Parse(userId));

            if (user == null)
            {
                return NotFound();
            }

            var model = new EditProfileRequest
            {
                Email = user.Email,
                UserName = user.Username,
                FullName = user.FullName,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(EditProfileRequest editProfileRequest)
        {
            if (!ModelState.IsValid)
            {
                return View(editProfileRequest);
            }

            var userId = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("SignIn");
            }

            var user = await _userRepository.GetByIdAsync(int.Parse(userId));

            if(user == null)
            {
                return NotFound();
            }

            user.Username = editProfileRequest.UserName;
            user.Email = editProfileRequest.Email;
            user.FullName = editProfileRequest.FullName;
            user.Address = editProfileRequest.Address;
            user.PhoneNumber = editProfileRequest.PhoneNumber;
            user.UpdatedAt = DateTime.Now;
            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            TempData["UpdateSuccess"] = "Profile updated successfully!";

            return RedirectToAction("EditProfile", "Account");
        }
    }
}
