using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aban.Common.Utility
{
	public static class GenericEnumList
	{
		public static List<SelectListItem> GetSelectValueEnum<T>() where T : struct, IConvertible
		{
			List<SelectListItem> list = new List<SelectListItem>();
			//list.Add(new SelectListItem
			//{
			//    Text = "نامشخص",
			//    Value = "0",
			//    Selected = true
			//});
			foreach (var eVal in Enum.GetValues(typeof(T)))
			{
#pragma warning disable CS8602 // Dereference of a possibly null reference.
				list.Add(new SelectListItem
				{
					Text = Enum.GetName(typeof(T), eVal).Replace("_", " "),
					Value = ((byte)eVal).ToString()
				});
#pragma warning restore CS8602 // Dereference of a possibly null reference.
			}

			return list;
		}
		public async static Task<List<SelectListItem>> GetSelectValueEnumAsync<T>() where T : struct, IConvertible
		{
			return await Task.Run(() =>
			{
				List<SelectListItem> list = new List<SelectListItem>();
				foreach (var eVal in Enum.GetValues(typeof(T)))
				{
#pragma warning disable CS8602 // Dereference of a possibly null reference.
					list.Add(new SelectListItem
					{
						Text = Enum.GetName(typeof(T), eVal).Replace("_", " "),
						Value = ((byte)eVal).ToString()
					});
#pragma warning restore CS8602 // Dereference of a possibly null reference.
				}

				return list;
			});
		}

		public static List<SelectListItem> GetSelectValueEnum<T>(string selectedValue) where T : struct, IConvertible
		{
			List<SelectListItem> list = new List<SelectListItem>();
			//list.Add(new SelectListItem
			//{
			//    Text = "نامشخص",
			//    Value = "0",
			//    Selected = true
			//});
			foreach (var eVal in Enum.GetValues(typeof(T)))
			{
#pragma warning disable CS8602 // Dereference of a possibly null reference.
				list.Add(new SelectListItem
				{
					Text = Enum.GetName(typeof(T), eVal).Replace("_", " "),
					Value = ((byte)eVal).ToString(),
					Selected = eVal.ToString() == selectedValue
				});
#pragma warning restore CS8602 // Dereference of a possibly null reference.
			}

			return list;
		}

		public static async Task<List<SelectListItem>> GetSelectValueEnumAsync<T>(string selectedValue) where T : struct, IConvertible
		{
			return await Task.Run(() =>
			{

				List<SelectListItem> list = new List<SelectListItem>();
				foreach (var eVal in Enum.GetValues(typeof(T)))
				{
#pragma warning disable CS8602 // Dereference of a possibly null reference.
					list.Add(new SelectListItem
					{
						Text = Enum.GetName(typeof(T), eVal).Replace("_", " "),
						Value = ((byte)eVal).ToString(),
						Selected = eVal.ToString() == selectedValue
					});
#pragma warning restore CS8602 // Dereference of a possibly null reference.
				}

				return list;
			});
		}
		public static List<SelectListItem> GetSelectValueEnum<T>(List<T> selectedValue) where T : struct, IConvertible
		{
			List<SelectListItem> list = new List<SelectListItem>();
			foreach (var eVal in Enum.GetValues(typeof(T)))
			{
				SelectListItem selectListItem = new SelectListItem();
#pragma warning disable CS8602 // Dereference of a possibly null reference.
				selectListItem.Text = Enum.GetName(typeof(T), eVal).Replace("_", " ");
#pragma warning restore CS8602 // Dereference of a possibly null reference.
				selectListItem.Value = ((byte)eVal).ToString();
				if (selectedValue.Count() > 0)
				{
					foreach (var item in selectedValue)
					{
						string value = Convert.ToInt32(item).ToString();
						if (selectListItem.Value == value)
						{
							selectListItem.Selected = true;
							break;
						}
						else
						{
							selectListItem.Selected = false;
						}
					}
				}
				else
				{
					selectListItem.Selected = false;
				}
				list.Add(selectListItem);
			}
			return list;
		}
		public static async Task<List<SelectListItem>> GetSelectValueEnumAsync<T>(List<T> selectedValue) where T : struct, IConvertible
		{
			return await Task.Run(() =>
			{
				List<SelectListItem> list = new List<SelectListItem>();
				foreach (var eVal in Enum.GetValues(typeof(T)))
				{
					SelectListItem selectListItem = new SelectListItem();
#pragma warning disable CS8602 // Dereference of a possibly null reference.
					selectListItem.Text = Enum.GetName(typeof(T), eVal).Replace("_", " ");
#pragma warning restore CS8602 // Dereference of a possibly null reference.
					selectListItem.Value = ((byte)eVal).ToString();
					if (selectedValue.Count() > 0)
					{
						foreach (var item in selectedValue)
						{
							string value = Convert.ToInt32(item).ToString();
							if (selectListItem.Value == value)
							{
								selectListItem.Selected = true;
								break;
							}
							else
							{
								selectListItem.Selected = false;
							}
						}
					}
					else
					{
						selectListItem.Selected = false;
					}
					list.Add(selectListItem);
				}
				return list;
			});
		}
		public static List<string> EnumReplaceUnderline<T>() => Enum.GetNames(typeof(T)).Select(x => x = x.Replace("_", " ")).ToList();


		public static TEnum Parse<TEnum>(string value) where TEnum : struct, IConvertible, IComparable, IFormattable
		{
			return (TEnum)Enum.Parse(typeof(TEnum), value, true);
		}
		//public static IEnumerable<SelectListItem> GetSelectValueEnum(string selectedValue, Domain.Entities.MachineBrand enumType)
		//{
		//    List<SelectListItem> list =
		//        (Enum.GetValues(typeof(Domain.Entities.MachineBrand)).Cast<Domain.Entities.MachineBrand>().Select(
		//       enu => new SelectListItem() { Text = enu.ToString(), Value = ((byte)enu).ToString() })).ToList();

		//    return list;
		//}


		//public static List<SelectListItem> GetSelectValueEnum<T>(string selectedValue) where T : struct, IConvertible
		//{
		//    List<SelectListItem> list = new List<SelectListItem>();
		//    //list.Add(new SelectListItem
		//    //{
		//    //    Text = "نامشخص",
		//    //    Value = "0",
		//    //    Selected = true
		//    //});
		//    foreach (var eVal in Enum.GetValues(typeof(T)))
		//    {
		//        list.Add(new SelectListItem
		//        {
		//            Text = Enum.GetName(typeof(T), eVal).Replace("_", " "),
		//            Value = eVal.ToString(),
		//            Selected = (eVal.ToString() == selectedValue) ? true : false
		//        });
		//    }

		//    return list;
		//}


	}
}
