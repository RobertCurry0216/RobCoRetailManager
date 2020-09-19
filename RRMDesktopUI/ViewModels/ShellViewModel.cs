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
        private SimpleContainer _container;
        private SalesViewModel salesVM;

        public ShellViewModel(IEventAggregator events, SalesViewModel sales, SimpleContainer container )
        {
            _container = container;
            _events = events;
            _events.Subscribe(this);

            salesVM = sales;

            ActivateItem(_container.GetInstance<LoginViewModel>());
        }

        public void Handle(LoginEvent message)
        {
            ActivateItem(salesVM);
        }
    }
}
