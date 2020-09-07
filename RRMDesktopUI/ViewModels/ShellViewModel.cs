using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRMDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>
    {
        private LoginViewModel loginVM;

        public ShellViewModel( LoginViewModel login )
        {
            loginVM = login;
            ActivateItem(loginVM);
        }
    }
}
