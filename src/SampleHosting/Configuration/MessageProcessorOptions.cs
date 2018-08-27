using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using SampleHosting.Configuration.Validation;

namespace SampleHosting.Configuration {
    public class MessageProcessorOptions : IValidatable {
        [Required]
        public string QueueName { get; set; }

        [Required]
        public string ConnectionString { get; set; }
    }
}