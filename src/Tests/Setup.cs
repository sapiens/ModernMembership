﻿using System;
using CavemanTools.Model.ValueObjects;
using CavemanTools.Web;
using Ploeh.AutoFixture;

namespace Tests
{
    public class Setup
    {
        static Fixture _fixture=new Fixture();
        public static Fixture GetAutoFixture()
        {
            return _fixture;
        }

        public static Guid Id=Guid.NewGuid();

        public static Email SomeEmail
        {
            get
            {
                return new Email("test@example.com");
            }
        }

        public static SetupPassword APassword
        {
            get
            {
                return new SetupPassword();
            }
        }
    }

    public class SetupPassword
    {
        public string Value = "pwd";
        public PasswordHash Hash
        {
            get
            {
                var str = new CavemanHashStrategy();
                return new PasswordHash(str.Hash(Value, "salt"), "salt");
            }
        }
    }
}