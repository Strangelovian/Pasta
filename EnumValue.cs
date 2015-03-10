    class ValueDescriptionCast
    {
        public Enum Value { get; set; }

        public string Description { get; set; }
    }

    class ValueDescription<TEnum> where TEnum : struct, IComparable, IFormattable, IConvertible
    {
        static ValueDescription()
        {
            if (typeof(TEnum).IsEnum) return;
            throw new ArgumentException(string.Format("ValueDescription TEnum type parameter must be an Enum Type, but {0} is not", typeof(TEnum).FullName));
        }

        public TEnum? Value { get; set; }

        public string Description { get; set; }

        public static implicit operator ValueDescription<TEnum>(Enum enumValue)
        {
            if (enumValue.GetType() != typeof(TEnum)) throw new ArgumentException(string.Format("illegal argument: enumValue parameter type must be {0}, but is {1} instead", enumValue.GetType().FullName, typeof(TEnum).FullName));
            return new ValueDescription<TEnum> { Value = (TEnum)Enum.ToObject(typeof(TEnum), enumValue), Description = enumValue.ToString() };
        }

        public Enum ToEnum()
        {
            return (Enum)Enum.ToObject(typeof(TEnum), Value);
        }
    }
