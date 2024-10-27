using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Security.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security.DataAccess.Configuration
{
	public class UserAppConfiguration : IEntityTypeConfiguration<UserApp>
	{
		public void Configure(EntityTypeBuilder<UserApp> builder)
		{
			builder.Property(x => x.City).HasMaxLength(30);
		}
	}
}
