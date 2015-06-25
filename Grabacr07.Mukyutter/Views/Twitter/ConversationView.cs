using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Grabacr07.Mukyutter.Views.Twitter
{
	class ConversationView : ListView
	{
		static ConversationView()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(ConversationView), new FrameworkPropertyMetadata(typeof(ConversationView)));
		}

	}
}
