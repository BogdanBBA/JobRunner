namespace WebAPI.Controllers.FoodDelivery
{
    public abstract class BaseOrganizer
    {
        public BaseOrganizer()
        {
            Initialize();
            Check();
        }

        protected abstract void Initialize();

        protected abstract void Check();
    }
}
