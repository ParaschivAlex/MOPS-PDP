using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDP.Controllers;
using PDP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PDP.Tests.Controllers
{
    [TestClass]
    public class SpecializationControllerTest
    {
        private Specialization spec = new Specialization { Name = "Neurologie", Price = 500 };
        private SpecializationsController specController = new SpecializationsController();

        [TestMethod]
        public void CreateGET()
        {
            //Arrange
            SpecializationsController sut = new SpecializationsController();

            //Act
            ViewResult result = sut.Create() as ViewResult; // incarc pagina de create si verific sa nu fie null

            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CreatePOST()
        {
            ViewResult spec_to_create = specController.Create(spec) as ViewResult; //apelez create pt o specializare

            ViewResult spec_from_index = specController.Index() as ViewResult; //iau tot indexul dupa create

            System.Diagnostics.Debug.Write("-----AICI E SPEC_FROM_INDEX------"); //pt debug
            System.Diagnostics.Debug.Write(spec_from_index);
            System.Diagnostics.Debug.Write("---------------------------------");

            List<Specialization> list_spec_from_index = spec_from_index.ViewData.Model as List<Specialization>; //incarc indexul intr-o lista

            Assert.IsTrue(list_spec_from_index.Contains(spec)); //verific daca in lista se afla ce am adaugat
        }


        [TestMethod]
        public void EditGET()
        {
            //Arrange
            SpecializationsController sut = new SpecializationsController();

            //Act
            ViewResult result = sut.Edit(1) as ViewResult; //incarc pagina de edit si verific sa nu fie null

            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void EditPUT()
        {
            Random rnd = new Random();
            int price = rnd.Next(1, 601); //generez random pretul intre 1 si 600 ca int

            ViewResult spec_from_index = specController.Index() as ViewResult; //iau iar toata lista de specializari

            List<Specialization> list_spec_from_index = spec_from_index.ViewData.Model as List<Specialization>; //si o fac lista

            System.Diagnostics.Debug.WriteLine("-----AICI E SPEC_FROM_INDEX------"); //pt debug
            System.Diagnostics.Debug.WriteLine(list_spec_from_index[0].SpecializationID.ToString());
            System.Diagnostics.Debug.WriteLine(list_spec_from_index[0].Name);
            System.Diagnostics.Debug.WriteLine(list_spec_from_index[0].Price.ToString());
            System.Diagnostics.Debug.WriteLine("---------------------------------");

            Specialization spec_for_editing = new Specialization { SpecializationID = 1, Name = "Marcel", Price = price };
            //mai sus declar in ce as vrea sa schimb o specializare

            ViewResult result = specController.Edit(list_spec_from_index.LastOrDefault().SpecializationID, spec_for_editing) as ViewResult;
            //apelez editul

            ViewResult spec_from_index_after_edit = specController.Index() as ViewResult;
            //iau noile specializari din index si le fac si lista

            List<Specialization> list_spec_from_index_after_edit = spec_from_index_after_edit.ViewData.Model as List<Specialization>;

            System.Diagnostics.Debug.WriteLine("-----AICI E SPEC_FROM_INDEX------"); //pt debug
            System.Diagnostics.Debug.WriteLine(list_spec_from_index_after_edit[0].SpecializationID.ToString());
            System.Diagnostics.Debug.WriteLine(list_spec_from_index_after_edit[0].Name);
            System.Diagnostics.Debug.WriteLine(list_spec_from_index_after_edit[0].Price.ToString());
            System.Diagnostics.Debug.WriteLine("---------------------------------");

            bool ok = false;
            //ma pregatesc sa verific daca exista specializarea schimbata
            foreach (Specialization viewSpecialization in list_spec_from_index_after_edit)
            {
                if (spec_for_editing.Name.Equals(viewSpecialization.Name)
                    && spec_for_editing.Price.Equals(viewSpecialization.Price)) // daca am o specializare cu acelasi nume si pret atunci e perfect
                {
                    ok = true;
                    break;
                }
            }
            Assert.IsTrue(ok);
        }

        [TestMethod]
        public void Delete()
        {
            ViewResult spec_from_index = specController.Index() as ViewResult; //iau iar toata lista de specializari

            List<Specialization> list_spec_from_index = spec_from_index.ViewData.Model as List<Specialization>; //si o fac lista

            ViewResult result = specController.Delete(list_spec_from_index.LastOrDefault().SpecializationID) as ViewResult; //dau delete la ultimul adaugat (cam ala ar trebui sa fie cel de la create)

            ViewResult spec_from_index_after_delete = specController.Index() as ViewResult;
            List<Specialization> list_spec_from_index_after_delete = spec_from_index_after_delete.ViewData.Model as List<Specialization>;

            Assert.IsNotNull(list_spec_from_index_after_delete); //iau lista noua si verific sa nu fie goala.

        }
    }
}
