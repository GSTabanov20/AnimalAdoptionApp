using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AnimalAdoption.Data;
using AnimalAdoption.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;

namespace AnimalAdoption.Controllers
{
    [Authorize]
    public class AnimalsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<User> _userManager;

        public AnimalsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, UserManager<User> userManager)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }

        // GET: Animals
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Animals.ToListAsync());
        }

        // GET: Animals/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animal = await _context.Animals
                .FirstOrDefaultAsync(m => m.Id == id);
            if (animal == null)
            {
                return NotFound();
            }

            return View(animal);
        }

        // GET: Animals/Create
        [Authorize(Policy = "RequireAdmin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Animals/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> Create([Bind("Id,Name,DateOfBirth,Species,Gender,Description,IsAdopted,IsAdmin")] Animal animal, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "animals", fileName);

                await using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
                animal.Image = fileName;
                _context.Add(animal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(animal);
        }

        // GET: Animals/Edit/5
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animal = await _context.Animals.FindAsync(id);
            if (animal == null)
            {
                return NotFound();
            }
            return View(animal);
        }

        // POST: Animals/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,DateOfBirth,Species,Gender,Description,IsAdopted,IsAdmin")] Animal animal, IFormFile? imageFile)
        {
            if (id != animal.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var currentAnimal = await _context.Animals.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
                    if (currentAnimal == null)
                    {
                        return NotFound();
                    }

                    if (imageFile != null)
                    {
                        var fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "animals", fileName);
                        
                        await using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }

                        if (currentAnimal.Image != null)
                        {
                            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "animals", currentAnimal.Image);
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }
                        animal.Image = fileName;
                    }
                    else
                    {
                        animal.Image = currentAnimal.Image;
                    }
                
                    _context.Update(animal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnimalExists(animal.Id))
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
            return View(animal);
        }

        // GET: Animals/Delete/5
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {   
                return NotFound();
            }

            var animal = await _context.Animals
                .FirstOrDefaultAsync(m => m.Id == id);
            if (animal == null)
            {
                return NotFound();
            }

            return View(animal);
        }

        // POST: Animals/Delete/5
        [Authorize(Policy = "RequireAdmin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var animal = await _context.Animals.FindAsync(id);
            if (animal != null)
            {
                var webRootPath = _webHostEnvironment.WebRootPath;
                if (animal.Image != null)
                {
                    var filePath = Path.Combine(webRootPath, "images", "animals", animal.Image);

                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
                _context.Animals.Remove(animal);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        // GET: Animals/AdoptForm/5
        public async Task<IActionResult> AdoptForm(int id)
        {
            var animal = await _context.Animals.FindAsync(id);
            if (animal == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);

            var adoptionRequest = new AdoptionRequest
            {
                AnimalId = animal.Id,
                UserId = user.Id
            };

            return View(adoptionRequest);
        }
        
        // POST: Animals/SubmitAdoptForm
        [HttpPost]
        public async Task<IActionResult> SubmitAdoptForm(int animalId, string question1, string question2, string question3)
        {
            var user = await _userManager.GetUserAsync(User);

            var formAnswers = new
            {
                question1,
                question2,
                question3
            };

            var adoptionRequest = new AdoptionRequest
            {
                AnimalId = animalId,
                UserId = user.Id,
                RequestDate = DateTime.Now,
                Status = "Pending",
                FormAnswers = JsonSerializer.Serialize(formAnswers)
            };

            _context.AdoptionRequests.Add(adoptionRequest);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = animalId });
        }
        
        // GET: Admin/AdoptionRequests
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> AdoptionRequests()
        {
            var requests = await _context.AdoptionRequests
                .Include(ar => ar.Animal)
                .Include(ar => ar.User)
                .ToListAsync();
            return View(requests);
        }

        // POST: Admin/AcceptRequest
        [Authorize(Policy = "RequireAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AcceptRequest(int id)
        {
            var request = await _context.AdoptionRequests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            request.Status = "Approved";
            var animal = await _context.Animals.FindAsync(request.AnimalId);
            if (animal != null)
            {
                animal.IsAdopted = true;
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AdoptionRequests));
        }

        // POST: Admin/DeclineRequest
        [Authorize(Policy = "RequireAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeclineRequest(int id)
        {
            var request = await _context.AdoptionRequests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            request.Status = "Rejected";
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AdoptionRequests));
        }
        
        public static string CalculateAge(DateOnly dateOfBirth)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var age = today.Year - dateOfBirth.Year;

            if (dateOfBirth > today.AddYears(-age))
                age--;

            var months = today.Month - dateOfBirth.Month;
            if (today.Day < dateOfBirth.Day)
                months--;

            if (months < 0)
            {
                months += 12;
            }
            
            var res = "";
            if (age == 0 && months == 0)
            {
                res = "newborn";
            }
            else if (age == 1)
            {
                res = "1 year";
            }
            else if (age > 1)
            {
                res = $"{age} years";
            }
            
            if (months == 1)
            {
                res += " 1 month";
            }
            else if (months > 1)
            {
                res += $" {months} months";
            }
            return res;
        }
        private bool AnimalExists(int id)
        {
            return _context.Animals.Any(e => e.Id == id);
        }
    }
}
