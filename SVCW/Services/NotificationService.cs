﻿using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SVCW.DTOs.Notifications;
using SVCW.Interfaces;
using SVCW.Models;

namespace SVCW.Services
{
	public class NotificationService : INotification
	{
		private readonly SVCWContext _context;

		public NotificationService(SVCWContext context)
		{
			this._context = context;
		}

        public async Task<bool> DeleteNoti(string notiId)
        {
            try
            {
                var db = this._context.Notification.Where(noti => noti.NotificationId.Equals(notiId));

                if (db != null)
                {
                    this._context.Notification.RemoveRange(db);
                    this._context.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public async Task<List<Notification>> GetNotifications(string userId)
        {
            try
            {
                var noti = await this._context.Notification.Where(x=>x.UserId.Equals(userId))
                    .Include(x=>x.Activity)
                    .OrderByDescending(x=>x.Datetime)
                    .ToListAsync();
                if (noti != null)
                {
                    return noti;
                }
                else
                {
                    throw new Exception("Không có thông báo");
                }
            }catch(Exception ex)
            {
                throw new Exception();
            }
        }

        public async Task<List<Notification>> markAsReadAll(string userId)
        {
            try
            {
                var notis = await this._context.Notification.Where(noti => noti.UserId.Equals(userId)).ToListAsync();
                if(notis != null)
                {
                    foreach (var item in notis)
                    {
                        item.Status = false;
                        this._context.Notification.Update(item);
                        await this._context.SaveChangesAsync();
                    }
                }
                return notis;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public async Task<bool> markAsRead(string notiId)
        {
            try
            {
                Notification nt = await this._context.Notification.Where(noti => noti.NotificationId.Equals(notiId)).FirstOrDefaultAsync();

                if (nt != null)
                {
                    nt.Status = false;
                    this._context.Notification.Update(nt);
                    return await this._context.SaveChangesAsync() >0;
                }
                else
                {
                    throw new Exception("Lỗi hệ thống");
                }
            }
            catch (Exception ex)
            {
                throw new Exception();
            }

        }

        public async Task<Notification> newNoti(NotificationDTO newNoti)
        {
            try
            {
                var noti = new Notification();

                noti.NotificationId = "NOT" + Guid.NewGuid().ToString().Substring(0, 7);
                noti.UserId = newNoti.UserId;
                noti.Title = newNoti.Title;
                noti.NotificationContent = newNoti.NotificationContent;
                noti.Datetime = DateTime.Now;
                noti.Status = false;

                await this._context.Notification.AddAsync(noti);
                await this._context.SaveChangesAsync();
                return noti;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Notification> UpdateNoti(string notiId, NotificationDTO notiInfo)
        {
            try
            {
                var newNoti = await this._context.Notification.Where(nt => nt.NotificationId.Equals(notiId)).FirstOrDefaultAsync();
                if (newNoti != null)
                {
                    if (notiInfo.UserId != null)
                        newNoti.UserId = notiInfo.UserId;
                    if (notiInfo.Title != null)
                        newNoti.Title = notiInfo.Title;
                    if (notiInfo.NotificationContent != null)
                        newNoti.NotificationContent = notiInfo.NotificationContent;
                    if (notiInfo.isRead != null)
                        newNoti.Status = (bool) notiInfo.isRead;

                    await this._context.SaveChangesAsync();
                }
                return newNoti;            
            }
             catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

