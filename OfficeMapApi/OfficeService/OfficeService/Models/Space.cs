using System;
using System.ComponentModel.DataAnnotations;

namespace OfficeService.Models
{
	public class Space
	{
		public Guid Guid { get; set; }
		public Guid SpaceGuid { get; set; }
	}
}
