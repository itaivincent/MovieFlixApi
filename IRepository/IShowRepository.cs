﻿using MovieFlixApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieFlixApi.IRepository
{
    interface IShowRepository
    {
        Task<Show> Get(int objId);

        Task<Show> Post();

        Task<Show> UpdateShow(int objId);
    }
}
