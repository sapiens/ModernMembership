﻿using CavemanTools;
using CavemanTools.Model.ValueObjects;
using CavemanTools.Web;
using ModernMembership;
using ModernMembership.Events;
using Xunit;
using System;
using System.Diagnostics;
using FluentAssertions;
using Xunit.Extensions;
using System.Linq;

namespace Tests.LocalMemberT
{
    public class ChangeLocalMember
    {
        private Stopwatch _t = new Stopwatch();
        private ModernMembership.LocalMember _sut;

        public ChangeLocalMember()
        {
            _sut = NewLocalMemberShould.CreateMember();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("            ")]
        public void display_name_accepts_empty_values(string value)
        {
            _sut.Invoking(s => s.DisplayName = value).ShouldNotThrow();
        }

        [Fact]
        public void changing_display_doesnt_generate_event()
        {
            _sut.DisplayName = "hey";
            var events= _sut.GetGeneratedEvents();
            events.Should().BeEmpty();
        }

        [Fact]
        public void changing_name_generates_event()
        {
            var name = new LoginName("chaged1");
            _sut.ChangeName(name);
            _sut.Name.Should().Be(name);
            _sut.GetGeneratedEvents().First().Cast<MemberLoginNameChanged>().NewName.Should().Be(_sut.Name.Value);
        }


        [Fact]
        public void changing_email_generates_event()
        {
            var email = new Email("g@example.com");
            _sut.ChangeEmail(email);
            _sut.Email.Should().Be(email);
            _sut.GetGeneratedEvents().First().Cast<MemberEmailChanged>().NewEmail.Should().Be(_sut.Email.Value);
        }

        [Fact]
        public void changing_password_generates_event()
        {
            var pwd = new PasswordHash("newh",Salt.Generate());
            _sut.ChangePassword(pwd);
            _sut.Password.Should().Be(pwd);
            _sut.GetGeneratedEvents().First().Cast<MemberPasswordChanged>().Should().NotBeNull();
        }

        [Fact]
        public void status_change_generates_corresponding_event()
        {
            _sut.Status=MemberStatus.Deleted;
            _sut.GetGeneratedEvents().First().Cast<MemberStatusChanged>().Status.Should().Be(MemberStatus.Deleted);
        }

        [Fact]
        public void status_cant_be_changed_to_undefined()
        {
            _sut.Invoking(sut => sut.Status = MemberStatus.Undefined).ShouldThrow<ArgumentException>();
        }

        protected void Write(object format, params object[] param)
        {
            Console.WriteLine(format.ToString(), param);
        }
    }
}