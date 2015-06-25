using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grabacr07.Mukyutter.ViewModels.Internal
{
	[AttributeUsage(AttributeTargets.Class)]
	class AutoGenerateAttribute : Attribute
	{
		public AutoGenerateAttribute()
		{
			
		}

		public Type TargetType { get; set; }

		//public string TargetTypeName { get; set; }

		//private Type _targetType;

		//public Type TargetType
		//{
		//	get { return _targetType; }
		//	set
		//	{
		//		_targetType = value;
		//		this.TargetTypeName = value.FullName;
		//	}
		//}

		//public string Test { get; set; }

		//public AutoGenerateAttribute()
		//{
		//	this.Test = "Test text!";
		//}
	}
}
