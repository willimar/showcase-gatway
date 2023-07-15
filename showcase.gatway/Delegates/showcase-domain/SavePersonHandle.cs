namespace showcase.gatway.Delegates.showcase_domain
{
    public class SavePersonHandle : DelegateBase
    {
        public SavePersonHandle(DelegateOption option) : base(option)
        {
            this.PrepareAction += this.InternalPrepareAction;
        }

        private void InternalPrepareAction(ref string actionName)
        {
            actionName = "save-person";
        }
    }
}
