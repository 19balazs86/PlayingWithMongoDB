using System;

namespace PlayingWithMongoDB.Types
{
  public interface IIdentifiable
    {
         Guid Id { get; }
    }
}