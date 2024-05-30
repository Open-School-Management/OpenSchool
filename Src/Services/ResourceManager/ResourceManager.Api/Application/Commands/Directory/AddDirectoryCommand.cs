using FluentValidation;
using Microsoft.Extensions.Localization;
using ResourceManager.Api.Application.DTOs;
using SharedKernel.Contracts;

namespace ResourceManager.Api.Application.Commands.Directory;

// [AuthorizationRequest(new ActionExponent[] { ActionExponent.Cloud })]
public class AddDirectoryCommand : BaseInsertCommand<DirectoryDto>
{
    public string Name { get; private set; }
    public string? ParentId { get; private set; }
}


public class AddDirectoryCommandValidator : AbstractValidator<AddDirectoryCommand>
{
    public AddDirectoryCommandValidator(IStringLocalizer<Resources> localizer)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(localizer["directory_name_must_not_be_empty"]);
        
        RuleFor(x => x.ParentId)
            .Must(p => string.IsNullOrEmpty(p) || Guid.TryParse(p, out var id)).WithMessage(localizer["directory_parent_id_is_invalid"]);
    }
}