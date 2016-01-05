namespace Meerkat.Mailer.Test
{
    public class CheckerFactory : NCheck.CheckerFactory
    {
        public CheckerFactory()
        {
            Initialize();
        }

        private void Initialize()
        {
            Register(typeof(CheckerFactory).Assembly);
        }
    }
}