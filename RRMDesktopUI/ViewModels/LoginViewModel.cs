﻿using Caliburn.Micro;
using RRMDesktopUI.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRMDesktopUI.ViewModels
{
    public class LoginViewModel : Screen
    {
        private string _userName;
        private IAPIHelper _apiHelper;

        public LoginViewModel(IAPIHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public string UserName
        {
            get { return _userName; }
            set { 
                _userName = value;
                NotifyOfPropertyChange(() => UserName);
                NotifyOfPropertyChange(() => CanLogin);
            }
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set { 
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

            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }
    }
}
