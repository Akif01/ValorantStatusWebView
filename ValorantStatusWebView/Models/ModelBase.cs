namespace ValorantStatusWebView.Models
{
    public abstract class ModelBase<TDto> where TDto : class
    {
        protected ModelBase(TDto dto)
        {
            Build(dto);
        }

        protected abstract void Build(TDto dto);
    }
}
