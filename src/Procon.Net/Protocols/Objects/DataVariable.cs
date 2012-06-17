﻿using System;
using System.ComponentModel;

namespace Procon.Net.Protocols.Objects
{
    [Serializable]
    public class DataVariable : INotifyPropertyChanged
    {
        // Public Accessors/Mutators.
        public String  Name
        {
            get { return mName; }
            set {
                if (mName != value) {
                    mName = value;
                    OnPropertyChanged("Name");
        } } }
        public Object  Value
        {
            get { return mValue; }
            set {
                if (mValue != value) {
                    mValue = value;
                    OnPropertyChanged("Value");
        } } }
        public Boolean IsReadOnly
        {
            get { return mIsReadOnly; }
            set {
                if (mIsReadOnly != value) {
                    mIsReadOnly = value;
                    OnPropertyChanged("IsReadOnly");
        } } }
        public String  FriendlyName
        {
            get { return mFriendlyName != null ? mFriendlyName : mName; }
            set {
                if (mFriendlyName != value) {
                    mFriendlyName = value;
                    OnPropertyChanged("FriendlyName");
        } } }
        public String  Description
        {
            get { return mDescription; }
            set {
                if (mDescription != value) {
                    mDescription = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        // Private Variables.
        private String mName;
        private Object mValue;
        private Boolean mIsReadOnly;
        private String mFriendlyName;
        private String mDescription;


        // Constructor.
        public DataVariable(String name = null, Object value = null, Boolean isReadOnly = false, String friendlyName = null, String description = null)
        {
            mName         = name;
            mValue        = value;
            mIsReadOnly   = isReadOnly;
            mFriendlyName = friendlyName;
            mDescription  = description;
        }
        
        // Safe Accessor.
        public T TryGetValue<T>(T fallback = default(T))
        {
            if (Value != null)
                return (T)Value;
            return fallback;
        }


        // Events.
        [field: NonSerialized]
        public    event PropertyChangedEventHandler PropertyChanged;
        protected void  OnPropertyChanged(String name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}