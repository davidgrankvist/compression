﻿using Compression.Lib.Encoders;
using Compression.Lib.Framework;

namespace Compression.Lib.Plugins
{
    internal class RunLengthDecoderCliPlugin : ICliEncoderPlugin
    {
        public string Name => "rle-d";

        public IEncoderMiddleware CreateEncoder()
        {
            return new RunLengthDecoder();
        }
    }
}
