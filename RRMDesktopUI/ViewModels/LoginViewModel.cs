﻿using Caliburn.Micro;
using RRMDesktopUI.EventModels;
using RRMDesktopUI.Library.Api;
using System;
using System.Threading.Tasks;

namespace RRMDesktopUI.ViewModels
{
    public class LoginViewModel : Screen
    {
        //TODO: remove hardcoded values at a later date.
        private string _userName = "abc@email.com";

        private IAPIHelper _apiHelper;
        private IEventAggregator _events;

        public LoginViewModel(IAPIHelper apiHelper, IEventAggregator events)
        {
            _apiHelper = apiHelper;
            _events = events;
        }

        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                NotifyOfPropertyChange(() => UserName);
                NotifyOfPropertyChange(() => CanLogin);
            }
        }

        private string _password = "123456789";

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => CanLogin);
            }
        }

        public bool CanLogin
        {
            get
            {
                if (UserName?.Length > 0 && Password?.Length > 0)
                {
                    return true;
                }
                return false;
            }
        }

        public bool IsErrorVisible
        {
            get => ErrorMessage?.Length > 0;
        }

        private string _ErrorMessage;

        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set
            {
                _ErrorMessage = value;
                NotifyOfPropertyChange(() => ErrorMessage);
                NotifyOfPropertyChange(() => IsErrorVisible);
            }
        }

        public async Task Login()
        {
            try
            {
                ErrorMessage = null;
                var result = await _apiHelper.Authenticate(UserName, Password);

                // capture user info
                await _apiHelper.GetLoggedInUserInfo(result.Access_Token);

                _events.PublishOnUIThread(new LoginEvent());
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }
    }
}