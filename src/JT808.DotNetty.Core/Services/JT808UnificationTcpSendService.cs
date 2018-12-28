﻿using DotNetty.Buffers;
using JT808.DotNetty.Abstractions.Dtos;
using JT808.DotNetty.Core;
using JT808.DotNetty.Core.Interfaces;
using System;

namespace JT808.DotNetty.Internal
{
    internal class JT808UnificationTcpSendService : IJT808UnificationTcpSendService
    {
        private readonly JT808TcpSessionManager jT808SessionManager;

        public JT808UnificationTcpSendService(JT808TcpSessionManager jT808SessionManager)
        {
            this.jT808SessionManager = jT808SessionManager;
        }

        public JT808ResultDto<bool> Send(string terminalPhoneNo, byte[] data)
        {
            JT808ResultDto<bool> resultDto = new JT808ResultDto<bool>();
            try
            {
                var session = jT808SessionManager.GetSession(terminalPhoneNo);
                if (session != null)
                {      
                    session.Channel.WriteAndFlushAsync(Unpooled.WrappedBuffer(data));
                    resultDto.Code = JT808ResultCode.Ok;
                    resultDto.Data = true;                    
                }
                else
                {
                    resultDto.Code = JT808ResultCode.Ok;
                    resultDto.Data = false;
                    resultDto.Message = "offline";
                }
            }
            catch (Exception ex)
            {
                resultDto.Data = false;
                resultDto.Code = JT808ResultCode.Error;
                resultDto.Message = Newtonsoft.Json.JsonConvert.SerializeObject(ex);
            }
            return resultDto;
        }
    }
}
