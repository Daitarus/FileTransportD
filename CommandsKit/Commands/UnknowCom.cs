﻿using CryptL;
using ProtocolCryptographyD;

namespace CommandsKit
{
    public class UnknowCom : Command
    {
        public UnknowCom(byte[]? bytes, byte[] sessionId)
        {
            if (sessionId == null)
                throw new ArgumentNullException(nameof(sessionId));
            if (sessionId.Length != LengthHash)
                throw new ArgumentOutOfRangeException($"{nameof(sessionId)} size must be {LengthHash}");

            typeCom = (byte)TypeCommand.UNKNOW;

            if (bytes != null)
            {
                payload = bytes;
            }

            this.sessionId = sessionId;
        }

        public static UnknowCom BytesToCom(byte[] payload)
        {
            if (payload == null)
                throw new ArgumentNullException(nameof(payload));
            if (payload.Length < LengthHash)
                throw new ArgumentOutOfRangeException($"{nameof(payload)} size must be more or equaly {LengthHash})");

            byte[] bytes = new byte[payload.Length - LengthHash];
            Array.Copy(payload, 0, bytes, 0, bytes.Length);

            byte[] sessionId = new byte[LengthHash];
            Array.Copy(payload, bytes.Length, sessionId, 0, sessionId.Length);

            return new UnknowCom(bytes, sessionId);
        }
    }
}