# MyStore — ASP.NET MVC5 + Entity Framework 6

Bộ mã nguồn khởi tạo theo bài giảng **Web BE Development — Project Setup**. Dùng **Visual Studio 2022 (Windows)** với workload **ASP.NET and web development** và **SQL Server**. Dự án chạy trên **.NET Framework 4.8**, **không phải ASP.NET Core**.

## Đã có gì?
- Solution `MaSV_LTW.sln` và web project MVC5 `MaSV_LTW`.
- Trang Customer (`/`): giao diện, danh sách sản phẩm, tìm kiếm, sắp xếp, phân trang và chi tiết sản phẩm (khi có dữ liệu SQL).
- Phân vùng `/Admin` với `Admin/HomeController`, trang Index và layout riêng.
- File tạo CSDL `database/MyStore.sql` dựa trên `database2024.txt` của giảng viên, đủ 6 bảng và khóa ngoại.
- Cấu hình route tránh trùng hai `HomeController`.
- Entity Framework 6 `DbContext`/mapping cho schema được giao (không phải tệp EDMX tự sinh).

## Cách chạy
1. Mở **SQL Server Management Studio (SSMS)**, chạy toàn bộ `database/MyStore.sql` bằng tài khoản có quyền tạo DB (chỉ chạy một lần; script tạo bảng không idempotent).
2. Mở `MaSV_LTW.sln` bằng **Visual Studio 2022**, chấp nhận **Restore NuGet Packages**.
3. Trong `MaSV_LTW/Web.config`, đổi `Data Source=localhost` thành server SQL của bạn, ví dụ `TênMáy\SQLEXPRESS`; mặc định sử dụng Windows Authentication. Không commit mật khẩu SQL.
4. Đặt `MaSV_LTW` làm **Startup Project**, nhấn **F5** (IIS Express). Trang chủ có thể chưa có sản phẩm vì database được giao chưa có bản ghi mẫu.
5. Test `/` và `/Admin`.

## Cách thực hiện đúng bước “EF Designer from Database” trong slide
Giảng viên yêu cầu **DB First qua tệp `.edmx`**. Để thực hiện phần này:
1. Kết nối database `MyStore` đã tạo.
2. Trong Visual Studio, chọn `Models` > **Add** > **New Item** > **ADO.NET Entity Data Model** > tên `MyStoreModel`.
3. Chọn **EF Designer from database**, tiếp tục chọn connection đến `MyStore`, chọn 6 bảng.
4. File mẫu hiện dùng `Models/MyStoreContext.cs` + `Models/Entities.cs` để cung cấp dữ liệu và chạy ngay. **Không dùng hai DbContext/model cùng tên gây xung đột.** Nếu sử dụng EDMX, cập nhật `Controllers/HomeController.cs` sang context và entities được sinh, hoặc bỏ mapping viết tay tương ứng.

## Phạm vi
Đây là **bộ project setup có thêm ví dụ hiển thị sản phẩm**, không phải dự án hoàn thiện tất cả yêu cầu Customer/Admin ở slide 2. Chưa có giỏ hàng, checkout, đăng ký/đăng nhập, CRUD admin hay phân quyền. Trang `/Admin` hiện chưa có kiểm tra đăng nhập — chỉ sử dụng trong môi trường học tập, không triển khai lên internet công khai.

## Đưa lên GitHub
Tạo repository trống trên GitHub rồi trong terminal tại thư mục này:
```bash
git init
git add .
git commit -m "Initial MVC5 MyStore project setup"
git branch -M main
git remote add origin https://github.com/USERNAME/REPOSITORY.git
git push -u origin main
```
Thay `USERNAME/REPOSITORY` bằng repository của bạn. Không đưa `bin/`, `obj/`, `.vs/`, tài khoản/mật khẩu database lên Git.


## Phần 2 — Admin Site (bổ sung 24/10/2026)
- Admin đăng nhập / đăng xuất (tài khoản vai trò A trong bảng [User]).
- Trang thống kê số lượng danh mục, sản phẩm, khách hàng, đơn hàng.
- CRUD danh mục (cảnh báo khi còn sản phẩm).
- CRUD sản phẩm; tìm kiếm theo từ khóa/danh mục, lọc khoảng giá, sắp xếp, phân trang.
- Xem/tìm kiếm/phân trang khách hàng và lịch sử đơn hàng; xem đơn hàng và chi tiết.
- Danh sách tài khoản quản trị và tìm kiếm; chưa có CRUD tài khoản quản trị.
- Nhập URL ảnh sản phẩm; **chưa hỗ trợ upload file ảnh lên server** như slide 34-36.

### Lưu ý an toàn / kiểm thử
CSDL do giảng viên cung cấp lưu `Password` ở dạng rõ; việc đăng nhập trong bài chỉ mang tính **minh họa tại máy học tập**, không triển khai lên máy chủ Internet. Trước khi triển khai thật phải chuyển sang mật khẩu băm an toàn, cookie xác thực chuẩn, phân quyền và bảo vệ session. Chưa kiểm thử build trên Windows/Visual Studio hay thử kết nối SQL Server tại môi trường này. Đây là bản code để mở/chạy thử, không cam kết không có lỗi.


## Bài BE3 – Product + Category (video #9, #10)
- Controller `Areas/Admin/Controllers/ProductsController.cs`: truy vấn Product kèm Category bằng Entity Framework `Include`, CRUD, lọc theo giá, tìm kiếm, sắp xếp, phân trang.
- Partial View `_ProductForm.cshtml`: dùng chung Create/Edit, dropdown Category, validation, input ảnh.
- Form Create/Edit dùng `multipart/form-data`, upload JPG/PNG/WebP tối đa 5MB vào `Content/images` với tên file ngẫu nhiên.
- Danh sách và trang chi tiết hiển thị ảnh sản phẩm.
- **Chưa được build/test trên Visual Studio Windows + SQL Server**; trước khi nộp cần mở `.sln`, cài NuGet, cấu hình DB, kiểm tra các action.
- Vẫn là dự án theo mô hình EF6 Code First mapping bảng có sẵn; **chưa phải EDMX Database First** theo slide buổi setup.


## BE4 — Tìm kiếm, sắp xếp, phân trang sản phẩm
- URL: `/Admin/Products` (đăng nhập Admin trước khi truy cập).
- Tìm từ khóa trong tên sản phẩm, mô tả và tên danh mục; có lọc giá min/max.
- Sắp xếp tên A-Z/Z-A, giá tăng/giảm; thêm ProductID để sắp xếp ổn định.
- Phân trang 10 sản phẩm mỗi trang; hiển thị tổng kết quả; điều hướng Trước/Sau; giữ bộ lọc khi chuyển trang.
- Code: `Areas/Admin/Controllers/ProductsController.cs`, `ViewModels/ProductListViewModel.cs`, `Areas/Admin/Views/Products/Index.cshtml`.
- Bài BE4 này kế thừa BE1–BE3, không cần tạo repository khác.
- **Chưa chạy build/test trên Windows Visual Studio + SQL Server**; phải chạy thử thực tế trước khi nộp.
