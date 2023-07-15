namespace showcase.gatway.Delegates.showcase_authenticate
{
    public class RegisterUserHandle : DelegateBase
    {
        public RegisterUserHandle(DelegateOption option) : base(option)
        {
            PrepareAction += InternalPrepareAction;
        }

        private void InternalPrepareAction(ref string actionName)
        {
            actionName = "register-user";
        }
    }
}
