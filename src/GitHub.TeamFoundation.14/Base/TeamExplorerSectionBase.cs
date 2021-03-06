﻿using System;
using GitHub.VisualStudio.Helpers;
using Microsoft.TeamFoundation.Controls;
using NullGuard;
using GitHub.Services;
using System.Diagnostics;
using GitHub.Api;
using GitHub.Models;
using GitHub.ViewModels;
using GitHub.Extensions;

namespace GitHub.VisualStudio.Base
{
    public class TeamExplorerSectionBase : TeamExplorerItemBase, ITeamExplorerSection, IServiceProviderAware
    {
        protected IConnectionManager connectionManager;

        bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set { isBusy = value; this.RaisePropertyChange(); }
        }

        bool isExpanded;
        public bool IsExpanded
        {
            get { return isExpanded; }
            set { isExpanded = value; this.RaisePropertyChange(); }
        }

        object sectionContent;
        [AllowNull]
        public object SectionContent
        {
            get { return sectionContent; }
            set { sectionContent = value; this.RaisePropertyChange(); }
        }

        string title;
        [AllowNull]
        public string Title
        {
            get { return title; }
            set { title = value; this.RaisePropertyChange(); }
        }

        [return: AllowNull]
        public virtual object GetExtensibilityService(Type serviceType)
        {
            return null;
        }

        public TeamExplorerSectionBase(ITeamExplorerServiceHolder holder)
            : base(holder)
        {
            IsVisible = false;
            IsEnabled = true;
            IsExpanded = true;
        }

        public TeamExplorerSectionBase(ISimpleApiClientFactory apiFactory, ITeamExplorerServiceHolder holder)
            : base(apiFactory, holder)
        {
            IsVisible = false;
            IsEnabled = true;
            IsExpanded = true;
        }

        public TeamExplorerSectionBase(ITeamExplorerServiceHolder holder, IConnectionManager cm) : this(holder)
        {
            connectionManager = cm;
        }

        public TeamExplorerSectionBase(ISimpleApiClientFactory apiFactory, ITeamExplorerServiceHolder holder,
            IConnectionManager cm) : this(apiFactory, holder)
        {
            connectionManager = cm;
        }

        void ITeamExplorerSection.Cancel()
        {
        }

        void ITeamExplorerSection.Initialize(object sender, SectionInitializeEventArgs e)
        {
            Initialize(e.ServiceProvider);
        }

        public virtual void Loaded(object sender, SectionLoadedEventArgs e)
        {
        }

        public virtual void Refresh()
        {
        }

        public virtual void SaveContext(object sender, SectionSaveContextEventArgs e)
        {
        }

        protected ITeamExplorerSection GetSection(Guid section)
        {
            return ServiceProvider.GetService<ITeamExplorerPage>()?.GetSection(section);
        }
    }
}
