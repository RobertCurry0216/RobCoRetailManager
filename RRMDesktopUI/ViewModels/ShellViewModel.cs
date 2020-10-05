using Caliburn.Micro;
using RRMDesktopUI.EventModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRMDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LoginEvent>
    {
        private IEventAggregator _events;
        private SalesViewModel salesVM;

        public ShellViewModel(IEventAggregator events, SalesViewModel sales)
        {
            _events = events;
            _events.Subscribe(this);

            salesVM = sales;

            ActivateItem(IoC.Get<LoginViewModel>());
        }

        public void Handle(LoginEvent message)
        {
            ActivateItem(salesVM);
        }
    }
}
