﻿using CavemanTools.Model.ValueObjects;
using CavemanTools.Web;
using ModernMembership;
using ModernMembership.Events;
using Xunit;
using System;
using System.Diagnostics;
using FluentAssertions;
using Xunit.Extensions;
using System.Linq;

namespace Tests
{
    public class ChangingLocalMember
    {
        private Stopwatch _t = new Stopwatch();
        private LocalMember _sut;

        public ChangingLocalMember()
        {
            _sut = NewLocalMemberShould.CreateMember();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("            ")]
        public void display_name_needs_a_non_empty_value(string value)
        {
            _sut.Invoking(s => s.DisplayName = value).ShouldThrow<FormatException>();        
        }

        [Fact]
        public void display_name_change_generates_event()
        {
            _sut.DisplayName = "hey";
            var events= _sut.GetGeneratedEvents();
            events.First().Cast<MemberNameChanged>().Name.Should().Be("hey");
        }

        [Fact]
        public void email_change_generates_event()
        {
            var email = new Email("g@example.com");
            _sut.ChangeEmail(email);
            _sut.Email.Should().Be(email);
            _sut.GetGeneratedEvents().First().Cast<MemberEmailChanged>().Email.Should().Be(_sut.Email.Value);
        }

        [Fact]
        public void password_change_generates_event()
        {
            var pwd = new PasswordHash("newh");
            _sut.ChangePassword(pwd);
            _sut.Password.Should().Be(pwd);
            _sut.GetGeneratedEvents().First().Cast<MemberPasswordChanged>().Should().NotBeNull();
        }

        protected void Write(object format, params object[] param)
        {
            Console.WriteLine(format.ToString(), param);
        }
    }
}