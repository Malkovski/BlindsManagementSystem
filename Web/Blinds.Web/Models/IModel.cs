namespace Blinds.Web.Models
{
    public interface IModel
    {
        void Init();
    }

    public interface IModel<TFilter>
    {
        void Init(TFilter data);
    }
}
