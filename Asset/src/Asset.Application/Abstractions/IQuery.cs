﻿using MediatR;

namespace Asset.Application.Abstractions;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
}
