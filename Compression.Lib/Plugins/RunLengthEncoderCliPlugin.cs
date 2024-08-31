﻿using Compression.Lib.Encoders;
using Compression.Lib.Framework;

namespace Compression.Lib.Plugins
{
    public class RunLengthEncoderCliPlugin : ICliEncoderPlugin
    {
        public string Name => "rle";

        public IEncoderMiddleware CreateEncoder()
        {
            return new RunLengthEncoder();
        }
    }
}
