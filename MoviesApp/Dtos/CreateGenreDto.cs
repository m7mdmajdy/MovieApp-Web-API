﻿using System.ComponentModel.DataAnnotations;

namespace MoviesApp.Dtos
{
    public class CreateGenreDto
    {
        [MaxLength(100)]
        public string Name{ get; set; }
    }
}
