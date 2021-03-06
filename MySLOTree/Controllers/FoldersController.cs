﻿using MySLOTree.Filters;
using MySLOTree.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MySLOTree.Controllers
{
    [InitializeSimpleNewsDB]
    public class FoldersController : Controller
    {
        public ActionResult Index()
        {
            using (TreeContext context = new TreeContext())
            {
                FoldersListModel model = new FoldersListModel()
                {
                    Folders = context.Folders.Where(x => !x.IsDeleted).ToArray().Select(x => new FoldersModel(x))
                };
                return View(model);//Возвращение список всех папок где параметр IsDeleted false

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(int? parentId, string title, bool isList)//метод добавления новой записи
        {

            using (TreeContext context = new TreeContext())
            {
                var newFolders = new Folders()
                {
                    ParentId = parentId,
                    Title = title,
                    IsList=isList                                   
                };
                context.Folders.Add(newFolders);
                context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]  
        public ActionResult Move(int nodeId, int? newParentId, bool list)//Метод перемещения ветви в другую ветвь
        {
            if (list)
            {
                return RedirectToAction("Index");
            }

            if (nodeId == newParentId)
            {
                return RedirectToAction("Index");
            }            
          
            using (TreeContext context = new TreeContext())
            {
                if (newParentId.HasValue && ContainsChilds(context, nodeId, newParentId.Value))
                {
                    return RedirectToAction("Index");
                }
                var node = context.Folders.Where(x => x.Id == nodeId).Single();
                node.ParentId = newParentId;
                context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        private bool ContainsChilds(TreeContext context, int parentId, int id)//не совсем понятно
        {
            bool result = false;
            var inner = context.Folders.Where(x => x.ParentId == parentId && !x.IsDeleted).ToArray();
            foreach (var node in inner)
            {
                if (node.Id == id && node.ParentId == parentId)
                {
                    return true;
                }
                result = ContainsChilds(context, node.Id, id);
            }

            return result;
        }

        [HttpPost]
        public ActionResult Delete(int id)//метод принимающий параметры для удаления записи
        {
            using (TreeContext context = new TreeContext())
            {
                DeleteNodes(context, id);
                context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        private void DeleteNodes(TreeContext context, int id)//метод удаления записи
        {
            var inner = context.Folders.Where(x => x.ParentId == id && !x.IsDeleted).ToArray();
            foreach (var node in inner)
            {
                node.IsDeleted = true;
                DeleteNodes(context, node.Id);
            }
            var deleted = context.Folders.Where(x => x.Id == id && !x.IsDeleted).Single();
            deleted.IsDeleted = true;
        }
    }
}
