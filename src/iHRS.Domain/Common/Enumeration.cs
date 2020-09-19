using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace iHRS.Domain.Common
{
    public abstract class Enumeration<TKey> : IComparable where TKey : IComparable, IEquatable<TKey>
    {
        public string Name { get; private set; }

        public TKey Id { get; }

        protected Enumeration(TKey id, string name)
        {
            Id = id;
            Name = name;
        }

        protected Enumeration()
        {

        }

        public override string ToString() => Name;

        public static IEnumerable<T> GetAll<T>() where T : Enumeration<TKey>
        {
            var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            return fields.Select(f => f.GetValue(null)).Cast<T>();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Enumeration<TKey> otherValue))
                return false;

            var typeMatches = GetType() == obj.GetType();
            var valueMatches = Id.Equals(otherValue.Id);

            return typeMatches && valueMatches;
        }

        public override int GetHashCode() => Id.GetHashCode();

        public static T FromValue<T>(TKey value) where T : Enumeration<TKey>
        {
            var matchingItem = Parse<T, TKey>(value, "value", item => item.Id.Equals(value));
            return matchingItem;
        }

        public static T FromDisplayName<T>(string displayName) where T : Enumeration<TKey>
        {
            var matchingItem = Parse<T, string>(displayName, "display name", item => item.Name == displayName);
            return matchingItem;
        }

        private static T Parse<T, TK>(TK value, string description, Func<T, bool> predicate) where T : Enumeration<TKey>
        {
            var matchingItem = GetAll<T>().FirstOrDefault(predicate);

            if (matchingItem == null)
                throw new InvalidOperationException($"'{value}' is not a valid {description} in {typeof(T)}");

            return matchingItem;
        }

        public int CompareTo(object other) => Id.CompareTo(((Enumeration<TKey>)other).Id);
    }

    public abstract class Enumeration : Enumeration<int>
    {
        protected Enumeration()
        {
        }

        protected Enumeration(int id, string name) : base(id, name)
        {
        }
    }
}