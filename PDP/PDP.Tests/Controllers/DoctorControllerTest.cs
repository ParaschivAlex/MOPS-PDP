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
    public class DoctorControllerTest
    {

        private Doctor doc = new Doctor { FirstName = "Ilie", SecondName = "Marinel", SpecializationID = 1, IsAvailable = true, PriceRate = 4, PhoneNumber = "0766554433", Email = "fiction@gmail.com", Photo = "https://img.freepik.com/premium-vector/cute-carrot-cartoon-carrot-clipart-vector-illustration_160901-2668.jpg?w=2000", Rating = 0 };

        private DoctorsController docController = new DoctorsController();

        [TestMethod]
        public void CreateGET()
        {
            //Arrange
            DoctorsController sut = new DoctorsController();

            //Act
            ViewResult result = sut.Create() as ViewResult; // incarc pagina de create si verific sa nu fie null

            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CreatePOST()
        {
            ViewResult doc_to_create = docController.Create(doc) as ViewResult; //apelez create pt doctor

            ViewResult doc_from_index = docController.Index(null, null, null) as ViewResult; //iau tot indexul dupa create

            System.Diagnostics.Debug.Write("-----AICI E DOC_FROM_INDEX------"); //pt debug
            System.Diagnostics.Debug.Write(doc_from_index);
            System.Diagnostics.Debug.Write("---------------------------------");

            List<Doctor> list_doc_from_index = doc_from_index.ViewData.Model as List<Doctor>; //incarc indexul intr-o lista

            Assert.IsTrue(list_doc_from_index.Contains(doc)); //verific daca in lista se afla ce am adaugat
        }

        [TestMethod]
        public void Search()
        {
            ViewResult doc_from_index = docController.Index(null, "Ilie", null) as ViewResult;
            List<Doctor> list_doc_from_index = doc_from_index.ViewData.Model as List<Doctor>;

            // Assert.IsTrue(indexDataSearchString.Contains(doc));
            Assert.AreEqual(list_doc_from_index[0].FirstName, doc.FirstName);
            //trebuie rulate in ordine, (new si dupaia search) a nu se rula editul inainte de search pentru ca nu mai gaseste.

        }

        [TestMethod]
        public void Sort()
        {
            ViewResult doc_from_index_unsorted = docController.Index(null, null, null) as ViewResult;
            List<Doctor> list_doc_from_index_unsorted = doc_from_index_unsorted.ViewData.Model as List<Doctor>;

            ViewResult doc_from_index_sorted = docController.Index("1", null, null) as ViewResult;
            List<Doctor> list_doc_from_index_sorted = doc_from_index_sorted.ViewData.Model as List<Doctor>;

            for (int i = 0; i < list_doc_from_index_unsorted.Count(); i++)
            {
                Assert.AreEqual(list_doc_from_index_sorted[i], list_doc_from_index_unsorted[i]);
            }

            ViewResult doc_from_index_unsorted2 = docController.Index(null, null, null) as ViewResult;
            List<Doctor> list_doc_from_index_unsorted2 = doc_from_index_unsorted2.ViewData.Model as List<Doctor>;
            list_doc_from_index_unsorted2.Reverse();

            ViewResult doc_from_index_sorted2 = docController.Index("2", null, null) as ViewResult;
            List<Doctor> list_doc_from_index_sorted2 = doc_from_index_sorted2.ViewData.Model as List<Doctor>;

            for (int i = 0; i < list_doc_from_index_unsorted.Count(); i++)
            {
                Assert.AreEqual(list_doc_from_index_sorted2[i], list_doc_from_index_unsorted2[i]);
            }


        }

        [TestMethod]
        public void EditGET()
        {
            //Arrange
            DoctorsController sut = new DoctorsController();

            ViewResult doc_from_index = docController.Index(null, null, null) as ViewResult; //iau tot indexul dupa create
            List<Doctor> list_doc_from_index = doc_from_index.ViewData.Model as List<Doctor>; //incarc indexul intr-o lista

            //Act
            ViewResult result = sut.Edit(list_doc_from_index.LastOrDefault().DoctorId) as ViewResult; //incarc pagina de edit si verific sa nu fie null

            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void EditPUT()
        {
            Random rnd = new Random();
            int price = rnd.Next(1, 11); //generez random price rate-ul intre 1 si 10 ca int

            ViewResult doc_from_index = docController.Index(null, null, null) as ViewResult; //iau tot indexul dupa create
            List<Doctor> list_doc_from_index = doc_from_index.ViewData.Model as List<Doctor>; //incarc indexul intr-o lista

            System.Diagnostics.Debug.WriteLine("-----AICI E doc_FROM_INDEX------"); //pt debug
            System.Diagnostics.Debug.WriteLine(list_doc_from_index[0].DoctorId.ToString());
            System.Diagnostics.Debug.WriteLine(list_doc_from_index[0].Specialization);
            System.Diagnostics.Debug.WriteLine(list_doc_from_index[0].FirstName);
            System.Diagnostics.Debug.WriteLine(list_doc_from_index[0].PriceRate.ToString());
            System.Diagnostics.Debug.WriteLine("---------------------------------");

            Doctor doc_for_editing = new Doctor { DoctorId = 1002, FirstName = "Ilie", SecondName = "Marinel", IsAvailable = true, PriceRate = price, PhoneNumber = "0766554433", Email = "fiction@gmail.com", Photo = "https://img.freepik.com/premium-vector/cute-carrot-cartoon-carrot-clipart-vector-illustration_160901-2668.jpg?w=2000", Rating = 0 };

            ViewResult result = docController.Edit(list_doc_from_index.LastOrDefault().DoctorId, doc_for_editing) as ViewResult;
            //apelez editul

            ViewResult doc_from_index_after_edit = docController.Index(null, null, null) as ViewResult;
            //iau noii doctori din index si ii fac si lista

            List<Doctor> list_doc_from_index_after_edit = doc_from_index_after_edit.ViewData.Model as List<Doctor>;

            System.Diagnostics.Debug.WriteLine("-----AICI E doc_FROM_INDEX------"); //pt debug
            System.Diagnostics.Debug.WriteLine(list_doc_from_index_after_edit[0].DoctorId.ToString());
            System.Diagnostics.Debug.WriteLine(list_doc_from_index_after_edit[0].Specialization);
            System.Diagnostics.Debug.WriteLine(list_doc_from_index[0].FirstName);
            System.Diagnostics.Debug.WriteLine(list_doc_from_index_after_edit[0].PriceRate.ToString());
            System.Diagnostics.Debug.WriteLine("---------------------------------");

            bool ok = false;
            //ma pregatesc sa verific daca exista specializarea schimbata
            foreach (Doctor viewDoctor in list_doc_from_index_after_edit)
            {
                if (doc_for_editing.FirstName.Equals(viewDoctor.FirstName)
                    && doc_for_editing.PriceRate.Equals(viewDoctor.PriceRate)) //un minim pt edit
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
            ViewResult doc_from_index = docController.Index(null, null, null) as ViewResult;

            List<Doctor> list_doc_from_index = doc_from_index.ViewData.Model as List<Doctor>;

            ViewResult result = docController.Delete(list_doc_from_index.LastOrDefault().DoctorId) as ViewResult; //dau delete la ultimul adaugat (cam ala ar trebui sa fie cel de la create)

            ViewResult spec_from_index_after_delete = docController.Index(null, null, null) as ViewResult;
            List<Doctor> list_doc_from_index_after_delete = spec_from_index_after_delete.ViewData.Model as List<Doctor>;

            Assert.IsNotNull(list_doc_from_index_after_delete); //iau lista noua si verific sa nu fie goala.

        }

    }
}
