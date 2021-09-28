using FluentValidation;
using EntityBase = Maxsys.ModelCore.EntityBase;

namespace Maxsys.MediaManager.CoreDomain.Validators
{
    /// <summary>
    /// Base class for entities validations.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class EntityValidator<T> : AbstractValidator<T>
        where T : EntityBase
    {
        /// <summary>
        /// Adds rule to validate <see cref="EntityBase.Id"/>.
        /// </summary>
        public void RuleForId()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}