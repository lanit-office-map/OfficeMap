using System;
using System.ComponentModel.DataAnnotations;

namespace OfficeService.Models
{

	public class Space
	{
		public Guid OfficeGuid { get; set; }
		public Guid SpaceGuid { get; set; }
	}
}
