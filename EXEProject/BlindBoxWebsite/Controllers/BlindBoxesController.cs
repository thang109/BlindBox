using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlindBoxWebsite.Models;

namespace BlindBoxWebsite.Controllers
{
    public class BlindBoxesController : Controller
    {
        private readonly BlindBoxContext _context;

        public BlindBoxesController(BlindBoxContext context)
        {
            _context = context;
        }

        // GET: BlindBoxes
        public async Task<IActionResult> Index()
        {
            return View(await _context.BlindBoxes.ToListAsync());
        }

        // GET: BlindBoxes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blindBox = await _context.BlindBoxes
                .FirstOrDefaultAsync(m => m.BlindBoxId == id);
            if (blindBox == null)
            {
                return NotFound();
            }

            return View(blindBox);
        }

        // GET: BlindBoxes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BlindBoxes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BlindBoxId,Name,Description,Price,Stock,ImageUrl,CreatedAt,UpdatedAt")] BlindBox blindBox)
        {
            if (ModelState.IsValid)
            {
                _context.Add(blindBox);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(blindBox);
        }

        // GET: BlindBoxes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blindBox = await _context.BlindBoxes.FindAsync(id);
            if (blindBox == null)
            {
                return NotFound();
            }
            return View(blindBox);
        }

        // POST: BlindBoxes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BlindBoxId,Name,Description,Price,Stock,ImageUrl,CreatedAt,UpdatedAt")] BlindBox blindBox)
        {
            if (id != blindBox.BlindBoxId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(blindBox);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlindBoxExists(blindBox.BlindBoxId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(blindBox);
        }

        // GET: BlindBoxes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blindBox = await _context.BlindBoxes
                .FirstOrDefaultAsync(m => m.BlindBoxId == id);
            if (blindBox == null)
            {
                return NotFound();
            }

            return View(blindBox);
        }

        // POST: BlindBoxes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blindBox = await _context.BlindBoxes.FindAsync(id);
            if (blindBox != null)
            {
                _context.BlindBoxes.Remove(blindBox);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlindBoxExists(int id)
        {
            return _context.BlindBoxes.Any(e => e.BlindBoxId == id);
        }
    }
}
