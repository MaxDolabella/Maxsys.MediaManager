using FluentValidation;
using Maxsys.ModelCore;

namespace Maxsys.MediaManager.CoreDomain.Validators;

/// <summary>
/// Base class for entities validations.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class EntityValidator<T> : AbstractValidator<T>
    where T : EntityBase
{
    /// <summary>
    /// Adds rule to validate <see cref="EntityBase">EntityBase.Id</see>.
    /// </summary>
    public EntityValidator<T> AddRuleForId()
    {
        RuleFor(x => x.Id).NotEmpty();

        return this;
    }
}