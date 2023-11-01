﻿using HouseRentingSystem.Common.Extensions;
using HouseRentingSystem.Services.Interfaces;
using HouseRentingSystem.ViewModels.House;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HouseRentingSystem.Controllers
{
    [Authorize]
    public class HouseController : Controller
    {
        private readonly IHouseService houseService;
        private readonly IAgentService agentService;
        public HouseController(IHouseService _houseService, IAgentService _agentService)
        {
            this.houseService = _houseService;
            this.agentService = _agentService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> All([FromQuery] AllHousesQueryModel query)
        {
            var queryResult = this.houseService.All(query.Category, query.SearchTerm, query.Sorting, query.CurrentPage, AllHousesQueryModel.HousesPerPage);

            query.TotalHousesCount = queryResult.TotalHousesCount;
            query.Houses = queryResult.Houses;

            List<string> houseCategories = (List<string>)await this.houseService.AllCategoriesNamesAsync();
            query.Categories = houseCategories;

            return View(query);
        }

        public async Task<IActionResult> Mine()
        {
            IEnumerable<HouseViewModel> houses = null;

            string currentUserId = this.User.Id();

            if (await this.agentService.ExistsById(currentUserId))
            {
                int agentId = this.agentService.GetAgentId(currentUserId);

                houses = await this.houseService.AllHousesByAgentIdAsync(agentId);
            }
            else
            {
                houses = await this.houseService.AllHousesByUserIdAsync(currentUserId);
            }

            return View(houses);
        }

        public async Task<IActionResult> Details(int id)
        {
            return View(new HouseDetailsViewModel());
        }

        public async Task<IActionResult> Add()
        {
            if (await this.agentService.ExistsById(User.Id()) == false)
            {
                return RedirectToAction(nameof(AgentController.Become), "Agent");
            }

            HouseFormModel model = new HouseFormModel();
            model.Categories = await this.houseService.AllCategoriesAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(HouseFormModel model)
        {
            if (await this.agentService.ExistsById(User.Id()) == false)
            {
                return RedirectToAction(nameof(AgentController.Become), "Agent");
            }

            if (await this.houseService.CategoryExistsAsync(model.CategoryId) == false)
            {
                ModelState.AddModelError(nameof(model.CategoryId),
                    "Category does not exist.");
            }

            if (!ModelState.IsValid)
            {
                model.Categories = await this.houseService.AllCategoriesAsync();
                return View(model);
            }

            int agentId = this.agentService.GetAgentId(User.Id());

            int newHouseId = await this.houseService.CreateAsync
                (model.Title, model.Address, model.Description,
                 model.ImageUrl, model.PricePerMonth, model.CategoryId,
                 agentId);

            return RedirectToAction(nameof(Details), new { id = newHouseId });
        }

        public async Task<IActionResult> Edit(int id)
        {
            return View(new HouseFormModel());
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, HouseFormModel model)
        {
            return RedirectToAction(nameof(Details), new { id = "1" });
        }

        public async Task<IActionResult> Delete(int id)
        {
            return View(new HouseDetailsViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Delete(HouseDetailsViewModel model)
        {
            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public async Task<IActionResult> Rent(int id)
        {
            return RedirectToAction(nameof(Mine));
        }

        [HttpPost]
        public async Task<IActionResult> Leave(int id)
        {
            return RedirectToAction(nameof(Mine));
        }
    }
}
