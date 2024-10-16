using BlindBoxWebsite.DTO.AccountDTOs;
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
                ModelState.AddModelError("Email", "Email đã được sử dụng!");
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

            await _userRepository.AddAccount(user);

            var confirmationLink = Url.Action("ConfirmEmail", "Account", new { userId = user.UserId, message = "Xác nhận email thành công, vui lòng đăng nhập!" }, Request.Scheme);

            var emailContent = new DTO.MailDTOs.MailContent
            {
                To = user.Email,
                Subject = "Chào mừng bạn đến với GBOX, xác nhận tài khoản và chúc bạn có một trải nghiệm tốt",
                Body = $@"
                <html>
                <head>
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            background-color: #f4f4f4;
                            margin: 0;
                            padding: 20px;
                        }}
                        .container {{
                            max-width: 600px;
                            margin: auto;
                            background: white;
                            padding: 20px;
                            border-radius: 8px;
                            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                        }}
                        h1 {{
                            color: #333;
                            text-align: center;
                        }}
                        p {{
                            font-size: 16px;
                            color: #555;
                            text-align: center; /* Căn giữa văn bản */
                        }}
                        .button {{
                            display: inline-block;
                            padding: 10px 20px;
                            font-size: 16px;
                            color: white;
                            background-color: #28a745;
                            text-decoration: none;
                            border-radius: 5px;
                            margin: 20px auto;
                            text-align: center;
                            display: block;
                            width: 200px;
                        }}
                        .footer {{
                            margin-top: 20px;
                            font-size: 12px;
                            color: #aaa;
                            text-align: center;
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <h1>Xác Nhận Email</h1>
                        <p>Để hoàn tất quá trình xác nhận tài khoản, vui lòng nhấn vào liên kết bên dưới. GBOX xin cảm ơn!</p>
                        <a href='{confirmationLink}' class='button'>Xác Nhận Email</a>
                        <div class='footer'>
                            <p>Cảm ơn bạn!</p>
                            <p>Đội ngũ hỗ trợ khách hàng GBOX</p>
                        </div>
                    </div>
                </body>
                </html>"
            };
            await _sendMailService.SendMail(emailContent);

            TempData["ConfirmMessage"] = "Vui lòng kiểm tra email của bạn để xác nhận đăng ký";

            return RedirectToAction("SignIn", "Account");
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                return NotFound("Không tìm thấy tài khoản!");
            }

            if (user.IsEmailConfirmed)
            {
                return RedirectToAction("SignIn", new { message = "Email của bạn đã được xác nhận, vui lòng đăng nhập!" });
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
                ModelState.AddModelError("Password", "Sai thông tin tài khoản hoặc mật khẩu!");
                return View(signInRequest);
            }

            if (!user.IsEmailConfirmed)
            {
                ViewData["ErrorMessage"] = "Tài khoản của bạn chưa được xác nhận, vui lòng kiểm tra email để xác nhận tài khoản!";
                return View(signInRequest);
            }
            HttpContext.Session.SetString("UserId", user.UserId.ToString());
            HttpContext.Session.SetString("UserName", user.Username);

            TempData["LogInSuccess"] = "Chào mừng bạn đến với GBOX Shop.";

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["LogoutSuccess"] = "Cảm ơn và hẹn gặp lại.";
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
                ModelState.AddModelError(string.Empty, "Không tìm thấy tài khoản.");
                return View(changePasswordRequest);
            }

            if (!BCrypt.Net.BCrypt.Verify(changePasswordRequest.OldPassword, user.Password))
            {
                ModelState.AddModelError(string.Empty, "Sai mật khẩu.");
                return View(changePasswordRequest);
            }

            if (changePasswordRequest.NewPassword != changePasswordRequest.ConfirmPassword)
            {
                ModelState.AddModelError(string.Empty, "Mật khẩu không khớp.");
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
                ModelState.AddModelError("Email", "Không tìm thấy email!");
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
                Subject = "Đặt Lại Mật Khẩu Của Bạn",
                Body = $@"
                <html>
                <head>
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            background-color: #f4f4f4;
                            margin: 0;
                            padding: 20px;
                        }}
                        .container {{
                            max-width: 600px;
                            margin: auto;
                            background: white;
                            padding: 20px;
                            border-radius: 8px;
                            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                        }}
                        h1 {{
                            color: #333;
                        }}
                        p {{
                            font-size: 16px;
                            color: #555;
                            text-align: center;
                        }}
                        .button {{
                            display: inline-block;
                            padding: 10px 20px;
                            font-size: 16px;
                            color: white;
                            background-color: #28a745;
                            text-decoration: none;
                            border-radius: 5px;
                            margin: 20px auto;
                            text-align: center;
                            display: block;
                            width: 200px;
                        }}
                        .footer {{
                            margin-top: 20px;
                            font-size: 12px;
                            color: #aaa;
                            text-align: center;
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <h1>Đặt lại Mật khẩu của bạn</h1>
                        <p>Xin chào,</p>
                        <p>Bạn đã yêu cầu đặt lại mật khẩu của mình. Vui lòng nhấp vào liên kết dưới đây để thiết lập mật khẩu mới:</p>
                        <p><a href='{resetPasswordLink}' class='button'>Đặt lại Mật khẩu</a></p>
                        <p>Nếu bạn không yêu cầu đặt lại mật khẩu, bạn có thể bỏ qua email này.</p>
                        <div class='footer'>
                            <p>Cảm ơn bạn!</p>
                            <p>Đội ngũ hỗ trợ khách hàng</p>
                        </div>
                    </div>
                </body>
                </html>"
            };

            await _sendMailService.SendMail(emailContent);
            TempData["ResetMessage"] = "Vui lòng kiểm tra email để tạo lại mật khẩu của bạn.";

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
                ModelState.AddModelError("NewPassword", "Mã token đã được sử dụng");
                return View(resetPasswordRequest);
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(resetPasswordRequest.NewPassword);
            user.ResetToken = null; 
            user.UpdatedAt = DateTime.Now;

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            TempData["ResetSuccess"] = "Cập nhật mật khẩu thành công, vui lòng đăng nhập.";

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

            TempData["UpdateSuccess"] = "Cập nhập thông tin thành công!";

            return RedirectToAction("EditProfile", "Account");
        }
    }
}
