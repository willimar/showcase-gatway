namespace showcase.gatway.Delegates.showcase_authenticate
{
    public class SaveAccountHandle : DelegateBase
    {
        public SaveAccountHandle(DelegateOption option) : base(option)
        {
            this.PrepareAction += this.InternalPrepareAction;
        }

        private void InternalPrepareAction(List<string> actionName)
        {
            actionName.Clear();
            actionName.Add("save-person");
            actionName.Add("register-account");
        }
    }
}
