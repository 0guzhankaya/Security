﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Security.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security.DataAccess.Configuration
{
	public class UserRefreshTokenConfiguration : IEntityTypeConfiguration<UserRefreshToken>
	{
		public void Configure(EntityTypeBuilder<UserRefreshToken> builder)
		{
			builder.HasKey(x => x.UserId);
			builder.Property(x => x.Code).IsRequired().HasMaxLength(128);
		}
	}
}
