﻿using GitHub.Primitives;
using NullGuard;
using System;
using System.Globalization;

namespace GitHub.Models
{
    public class RepositoryModel : SimpleRepositoryModel, IRepositoryModel,
        IEquatable<RepositoryModel>, IComparable<RepositoryModel>
    {
        public RepositoryModel(long id, string name, UriString cloneUrl, bool isPrivate, bool isFork,  IAccount ownerAccount)
            : base(name, cloneUrl)
        {
            Id = id;
            OwnerAccount = ownerAccount;
            IsFork = isFork;
            SetIcon(isPrivate, isFork);
            // this is an assumption, we'd have to load the repo information from octokit to know for sure
            // probably not worth it for this ctor
            DefaultBranch = new BranchModel("master", this);
        }

        public RepositoryModel(Octokit.Repository repository)
            : base(repository.Name, repository.CloneUrl)
        {
            Id = repository.Id;
            IsFork = repository.Fork;
            SetIcon(repository.Private, IsFork);
            OwnerAccount = new Account(repository.Owner);
            DefaultBranch = new BranchModel(repository.DefaultBranch, this);
            Parent = repository.Parent != null ? new RepositoryModel(repository.Parent) : null;
            if (Parent != null)
                Parent.DefaultBranch.DisplayName = Parent.DefaultBranch.Id;
        }

#region Equality Things
        public void CopyFrom(IRepositoryModel other)
        {
            if (!Equals(other))
                throw new ArgumentException("Instance to copy from doesn't match this instance. this:(" + this + ") other:(" + other + ")", nameof(other));
            Icon = other.Icon;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals([AllowNull]object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            var other = obj as RepositoryModel;
            return Equals(other);
        }

        public bool Equals([AllowNull]IRepositoryModel other)
        {
            if (ReferenceEquals(this, other))
                return true;
            return other != null && Id == other.Id;
        }

        public bool Equals([AllowNull]RepositoryModel other)
        {
            if (ReferenceEquals(this, other))
                return true;
            return other != null && Id == other.Id;
        }

        public int CompareTo([AllowNull]IRepositoryModel other)
        {
            return other != null ? UpdatedAt.CompareTo(other.UpdatedAt) : 1;
        }

        public int CompareTo([AllowNull]RepositoryModel other)
        {
            return other != null ? UpdatedAt.CompareTo(other.UpdatedAt) : 1;
        }

        public static bool operator >([AllowNull]RepositoryModel lhs, [AllowNull]RepositoryModel rhs)
        {
            if (ReferenceEquals(lhs, rhs))
                return false;
            return lhs?.CompareTo(rhs) > 0;
        }

        public static bool operator <([AllowNull]RepositoryModel lhs, [AllowNull]RepositoryModel rhs)
        {
            if (ReferenceEquals(lhs, rhs))
                return false;
            return (object)lhs == null || lhs.CompareTo(rhs) < 0;
        }

        public static bool operator ==([AllowNull]RepositoryModel lhs, [AllowNull]RepositoryModel rhs)
        {
            return ReferenceEquals(lhs, rhs);
        }

        public static bool operator !=([AllowNull]RepositoryModel lhs, [AllowNull]RepositoryModel rhs)
        {
            return !(lhs == rhs);
        }
#endregion

        public IAccount OwnerAccount { get; }
        public long Id { get; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public bool IsFork { get; }
        [AllowNull] public IRepositoryModel Parent { [return: AllowNull] get; }
        public IBranch DefaultBranch { get; }

        internal string DebuggerDisplay
        {
            get
            {
                return String.Format(CultureInfo.InvariantCulture,
                    "{5}\tId: {0} Name: {1} CloneUrl: {2} LocalPath: {3} Account: {4}", Id, Name, CloneUrl, LocalPath, Owner, GetHashCode());
            }
        }
    }
}
