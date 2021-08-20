using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using GoogleReCaptcha.V3.Interface;
using KristaShop.Common.Enums;
using KristaShop.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Module.App.Business.Interfaces;
using Module.App.Business.Models;
using Module.App.WebUI.Models;
using Module.Common.WebUI.Base;
using Module.Common.WebUI.Extensions;
using Module.Common.WebUI.Infrastructure;
using Serilog;

namespace Module.App.WebUI.Controllers {
    [Permission]
    [Authorize(AuthenticationSchemes = GlobalConstant.Session.AllFrontSchemes)]
    public class FeedbackController : AppControllerBase {
        private readonly IFeedbackService _feedbackService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly ICaptchaValidator _captchaValidator;
        private readonly GlobalSettings _globalSettings;

        public FeedbackController(IFeedbackService feedbackService, IMapper mapper, ILogger logger,
            IOptions<GlobalSettings> _globalSettingsOptions, ICaptchaValidator captchaValidator) {
            _feedbackService = feedbackService;
            _mapper = mapper;
            _logger = logger;
            _captchaValidator = captchaValidator;
            _globalSettings = _globalSettingsOptions.Value;
        }

        [AllowAnonymous]
        public IActionResult Get() {
            var model = UserSession?.User != null ? _mapper.Map<FeedbackViewModel>(UserSession.User) : new FeedbackViewModel();
            return PartialView("_FeedbackFormPartial", model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FeedbackViewModel model) {
            if (ModelState.IsValid) {
                if (await _captchaValidator.IsCaptchaPassedAsync(model.Captcha)) {
                    try {
                        var feedbackModel = _mapper.Map<FeedbackDTO>(model);
                        feedbackModel.Type = FeedbackType.Basic;
                        var result = await _feedbackService.InsertFeedback(feedbackModel);

                        if (!result.IsSuccess) {
                            ModelState.TryAddModelErrors(result.Messages);
                        }
                    } catch (Exception ex) {
                        _logger.Error(ex, "Failed to create feedback. {message}", ex.Message);
                        ModelState.TryAddModelErrors(OperationResult.Failure().Messages);
                    }
                } else {
                    ModelState.AddModelError(string.Empty, "Капча не валидна");
                }
            }

            return PartialView("_FeedbackFormPartial", model);
        }

        public IActionResult GetWithFile() {
            var model = new AuthFeedbackViewModel();
            return PartialView("_FeedbackCoplaintsPartial", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateWithFile(AuthFeedbackViewModel model) {
            if (ModelState.IsValid) {
                try {
                    var feedbackModel = _mapper.Map<FeedbackDTO>(UserSession.User);
                    _mapper.Map(model, feedbackModel);
                    feedbackModel.Type = FeedbackType.ComplaintsAndSuggestions;

                    OperationResult result;
                    if (model.File != null) {
                        var file = await _getFeedbackFileDto(model);
                        result = await _feedbackService.InsertFeedbackWithFileAsync(feedbackModel, file);
                    } else {
                        result = await _feedbackService.InsertFeedback(feedbackModel);
                    }

                    if (!result.IsSuccess) {
                        ModelState.TryAddModelErrors(result.Messages);
                    }
                } catch (Exception ex) {
                    _logger.Error(ex, "Failed to create feedback. {message}", ex.Message);
                    ModelState.TryAddModelErrors(OperationResult.Failure().Messages);
                }
            }

            return PartialView("_FeedbackCoplaintsPartial", model);
        }

        private async Task<FeedbackCreateFileDTO> _getFeedbackFileDto(AuthFeedbackViewModel model) {
            var file = new FeedbackCreateFileDTO {
                OriginalName = model.File.FileName,
                FileStream = new MemoryStream(),
                FilesDirectoryPath = _globalSettings.FilesDirectoryPath,
                Directory = _globalSettings.FeedbackFilesDirectory
            };
            await model.File.CopyToAsync(file.FileStream);
            return file;
        }

        public IActionResult GetManagementContacts() {
            return PartialView("_FeedbackManagementContactsPartial", new ManagementContactsFeedbackViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateManagementContacts(ManagementContactsFeedbackViewModel model) {
            if (ModelState.IsValid) {
                try {
                    var feedbackModel = _mapper.Map<FeedbackDTO>(UserSession.User);
                    _mapper.Map(model, feedbackModel);
                    feedbackModel.Type = FeedbackType.ManagementContacts;
                    var result = await _feedbackService.InsertFeedback(feedbackModel);

                    if (!result.IsSuccess) {
                        ModelState.TryAddModelErrors(result.Messages);
                    }
                } catch (Exception ex) {
                    _logger.Error(ex, "Failed to create feedback. {message}", ex.Message);
                    ModelState.TryAddModelErrors(OperationResult.Failure().Messages);
                }
            }

            return PartialView("_FeedbackManagementContactsPartial", model);
        }
    }
}
