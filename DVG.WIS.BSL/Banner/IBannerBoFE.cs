﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Business.Banner
{
	public interface IBannerBoFE
	{
		List<Entities.Banner> GetAllActive();
	}
}
