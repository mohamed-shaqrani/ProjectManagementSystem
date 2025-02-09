﻿using FluentValidation;
using ProjectManagementSystem.Api.Entities;

namespace ProjectManagementSystem.Api.Features.TasksManagement.Tasks.AddTask
{
    public record AddTaskRequestViewModel(string Title, string Description, ProjectTaskStatus Status, int UserID, int ProjectID);

    public class AddTaskRequestViewModelValidator : AbstractValidator<AddTaskRequestViewModel>
    {
        public AddTaskRequestViewModelValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Not empty");
        }
    }
}
