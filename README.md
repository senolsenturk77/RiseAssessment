# Assessment

Projenin amacı farklı microservisleri haberleştirerek, gelen rapor taleplerinin bir mesaj kuyruğu aracılığı ile değerlendirilerek, raporların excel dosyası olarak hazırlanmasını sağlamaktır.

# Kullanılan Teknolojiler

 * .Net Core Web API
 * PostgreSQL 
 * RabbitMQ
 * EPPlus (Excel Rapor için)
 * xUnit
 * Moq
 * Fine Code Coverage
 
# Proje Detayları 
  * Projede 2 adet microservice bulunmaktadır. 
    * ContactService : Rehber işlemleri, yeni kişi ve iletişim bilgisi ekleme, silme, güncelleme ve sorgulama işlevlerini barındırır.
    * ReportService : Rehber içerisinde bulunan kayıtlar ile ilgili rapor talebinde bulunma, rapor isteğinin durumunu izleme ve oluşturulan raporu 
      indirme işlevlerini barındırır.
  * İki serviste veritabanı olarak PostgreSQL veritabanı kullanmaktadır. Her servisin kendine ait veritabanı bulunmaktadır. 
  * Projede Message broker olarak RabbitMQ kullanılmıştır. 
  * Projede master ve development brachları olmak üzere iki adet brach bulunmaktadır.
  
  
# Servisler Hakkında

## ContactService
  * Adres defterindeki kişi kayıtlarını tümünü listelemek için /api/Contact GET
  * Adres defterindeki belirli bir kişi kayıdını dönmek için /api/Contact/{id} GET (id= Guid)
  * Yeni kişi kaydı oluşturmak için  /api/Contact POST işlemi kullanılır. (Parametre gövde verisinde ContactDTO)
  * Var olan bir kaydı güncellemek için /api/Contact PUT (Parametre gövde verisinde ContactDTO)
  * Adres defterindeki belirli bir kişi kayıdını silmek için /api/Contact/{id} DELETE (id= Guid)
  * Adres defterindeki bir kişi kaydını iletişim bilgileri ile birlikte almak için **/api/Contact/WithInfo/{contactId}** (contactId= Guid)
  * Adres defterindeki tüm iletişim verilerini listelemek için /api/ContactInfo GET
  * Adres defterindeki belirli bir iletişim verilerisini listelemek için /api/ContactInfo/{id} GET (id= Guid)
  * Adres defterindeki belirli bir iletişim verilerisini silmek için /api/ContactInfo/{id} DELETE (id= Guid)
  * Adres defterindeki bir iletişim verilerisini güncelemek için /api/ContactInfo PUT (Parametre gövde verisinde ContactInfoDTO)
  * Adres defterinde belirli bir kişiye ait tüm iletişim verilerini listelemek için **/api/ContactInfo/Contact/{contactId}** (contactId= Guid)

## ReportService
  * Tüm rapor isteklerinin durum kayıtlarını listelemek için /api/Report GET
  * Yeni bir rapor isteği oluşturmak için /api/Report POST (parametre yok)
  * Belirli bir raporu indirmek için /api/Report/{uuid} (uuid= Guid, listedeki istek kayıt id'si)




 Saygılarımla,
 Şenol Şentürk,
