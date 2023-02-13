using EntityFrameworkClassLibrary.UnitOfWork;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoListApp.Dto;
using ToDoListApp.Service;

namespace ToDoListApp.Service
{
    internal class Service : IService
    {
        private IUnitOfWork _unitOfWork { get; set; }
        private IMapper _mapper { get; set; }
        public Service(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task AddTodo(Todo todo)
        {
            Todo newTodo = _mapper.Map<Todo>(todo);

            await _unitOfWork.TodoRepository.AddTodo(newTodo);
            await _unitOfWork.Save();
            _unitOfWork.Dispose();
        }

        public async Task<IActionResult> DeleteTodo(string id)
        {
            Todo dbTodo = await _unitOfWork.TodoRepository.GetTodoById(id);

            if (dbTodo == null)
                return new NotFoundObjectResult(dbTodo);

            _unitOfWork.TodoRepository.DeleteTodo(dbTodo);
            await _unitOfWork.Save();
            _unitOfWork.Dispose();

            TodoDto todoDeleted = _mapper.Map<TodoDto>(dbTodo);

            return new OkObjectResult(todoDeleted);
        }

        public Task<IEnumerable<Todo>> GetAllTodos()
        {
            throw new NotImplementedException();
        }

        public Task<Todo> GetTodoById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> UpdateTodo(Todo todo)
        {
            throw new NotImplementedException();
        }
    }
}


//using EntityFrameworkClassLibrary.Models;
//using EntityFrameworkClassLibrary.UnitOfWork;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Mapster;
//using Dtos.CustomerResponse;
//using Dtos.ModelRequest;
//using MapsterMapper;

//namespace ApiFunctionWithRepositoryPattern.LogicBusiness
//{
//    public class Service : IService
//    {
//        public IUnitOfWork _unitOfWork { get; set; }
//        public IMapper _mapper { get; set; }

//        public Service(IUnitOfWork unitOfWork, IMapper mapper)
//        {
//            _unitOfWork = unitOfWork;
//            _mapper = mapper;
//        }

//        public async Task AddCustomer(CustomerRequest customerReq)
//        {
//            Customer customer = _mapper.Map<Customer>(customerReq);

//            await _unitOfWork.CustomerRepository.AddCustomer(customer);
//            await _unitOfWork.Save();
//            _unitOfWork.Dispose();
//        }

//        public async Task<List<CustomerResponse>> GetAllCustomers()
//        {
//            List<CustomerResponse> customersDto = new List<CustomerResponse>();
//            var customers = await _unitOfWork.CustomerRepository.GetAllCustomers();

//            if (customers == null)
//                return null;

//            foreach (var customer in customers)
//            {
//                CustomerResponse customerDto = _mapper.Map<CustomerResponse>(customer);
//                customersDto.Add(customerDto);
//            }

//            _unitOfWork.Dispose();

//            return customersDto;
//        }

//        public async Task<CustomerResponse> GetCustomerById(int id)
//        {
//            Customer customer = await _unitOfWork.CustomerRepository.GetCustomerById(id);

//            if (customer == null)
//                return null;

//            CustomerResponse customerDto = _mapper.Map<CustomerResponse>(customer);

//            _unitOfWork.Dispose();

//            return customerDto;
//        }

//        public async Task<CustomerResponse> UpdateCustomer(CustomerRequest customerReq, int id)
//        {
//            Customer dbCustomer = await _unitOfWork.CustomerRepository.GetCustomerById(id);

//            if (dbCustomer == null)
//                return null;

//            dbCustomer.SurName = customerReq.SurName;
//            dbCustomer.LastName = customerReq.LastName;
//            _unitOfWork.CustomerRepository.UpdateCustomer(dbCustomer);

//            await _unitOfWork.Save();
//            _unitOfWork.Dispose();

//            var customerDto = _mapper.Map<CustomerResponse>(dbCustomer);

//            return customerDto;
//        }

//        public async Task<CustomerResponse> RemoveCustomer(int id)
//        {
//            Customer dbCustomer = await _unitOfWork.CustomerRepository.GetCustomerById(id);

//            if (dbCustomer == null)
//                return null;

//            _unitOfWork.CustomerRepository.RemoveCustomer(dbCustomer);
//            await _unitOfWork.Save();
//            _unitOfWork.Dispose();

//            CustomerResponse customerDto = _mapper.Map<CustomerResponse>(dbCustomer);

//            return customerDto;
//        }

//        public async Task AddInvoice(InvoiceRequest invoiceReq)
//        {
//            Invoice invoice = _mapper.Map<Invoice>(invoiceReq);

//            await _unitOfWork.InvoiceRepository.AddInvoice(invoice);
//            await _unitOfWork.Save();
//            _unitOfWork.Dispose();
//        }

//        public async Task<List<InvoiceResponse>> GetAllInvoices()
//        {
//            List<InvoiceResponse> invoicesDto = new List<InvoiceResponse>();
//            var invoices = await _unitOfWork.InvoiceRepository.GetAllInvoices();

//            if (invoices == null)
//                return null;

//            foreach (var invoice in invoices)
//            {
//                InvoiceResponse invoiceDto = _mapper.Map<InvoiceResponse>(invoice);
//                invoicesDto.Add(invoiceDto);
//            }

//            _unitOfWork.Dispose();

//            return invoicesDto;
//        }

//        public async Task<InvoiceResponse> GetInvoiceById(int id)
//        {
//            Invoice invoice = await _unitOfWork.InvoiceRepository.GetInvoiceById(id);

//            if (invoice == null)
//                return null;

//            InvoiceResponse invoiceDto = _mapper.Map<InvoiceResponse>(invoice);

//            _unitOfWork.Dispose();

//            return invoiceDto;
//        }


//        public async Task<InvoiceResponse> UpdateInvoice(InvoiceRequest invoiceReq, int id)
//        {
//            Invoice dbInvoice = await _unitOfWork.InvoiceRepository.GetInvoiceById(id);

//            if (dbInvoice == null)
//                return null;

//            dbInvoice.OrderDate = invoiceReq.OrderDate;
//            dbInvoice.Quantity = invoiceReq.Quantity;
//            dbInvoice.Price = invoiceReq.Price;
//            dbInvoice.CustomerId = invoiceReq.CustomerId;
//            _unitOfWork.InvoiceRepository.UpdateInvoice(dbInvoice);

//            await _unitOfWork.Save();
//            _unitOfWork.Dispose();

//            InvoiceResponse invoiceDto = _mapper.Map<InvoiceResponse>(dbInvoice);

//            return invoiceDto;
//        }

//        public async Task<InvoiceResponse> RemoveInvoice(int id)
//        {
//            Invoice dbInvoice = await _unitOfWork.InvoiceRepository.GetInvoiceById(id);

//            if (dbInvoice == null)
//                return null;

//            _unitOfWork.InvoiceRepository.RemoveInvoice(dbInvoice);
//            await _unitOfWork.Save();
//            _unitOfWork.Dispose();

//            InvoiceResponse invoiceDto = _mapper.Map<InvoiceResponse>(dbInvoice);

//            return invoiceDto;
//        }

//        public async Task AddProduct(ProductRequest productReq)
//        {
//            Product product = _mapper.Map<Product>(productReq);

//            await _unitOfWork.ProductRepository.AddProduct(product);
//            await _unitOfWork.Save();
//            _unitOfWork.Dispose();
//        }

//        public async Task<ProductResponse> RemoveProduct(int id)
//        {
//            Product dbProduct = await _unitOfWork.ProductRepository.GetProduct(id);

//            if (dbProduct == null)
//                return null;

//            _unitOfWork.ProductRepository.RemoveProduct(dbProduct);
//            await _unitOfWork.Save();
//            _unitOfWork.Dispose();

//            ProductResponse productDto = _mapper.Map<ProductResponse>(dbProduct);

//            return productDto;
//        }
//    }
//}

