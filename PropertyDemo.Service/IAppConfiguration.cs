using System;

namespace PropertyDemo.Service
{
  public interface IAppConfiguration
  {
    string ConnectionString { get; }
  }
}
