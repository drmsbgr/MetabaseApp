# Metabase Embedded App

- Bu projede self-hosted Metabase üzerinde oluşturulan dashboard'ları bir web uygulamasına gömme işlemi gerçekleştirildi.

# Metabase platformunun kurulması

- Docker kullanılarak metabase imajı yüklendi.
- Ardından bu imajdan bir container oluşturuldu.
- Metabase kurulum işlemi gerçekleştirildi.
- Metabase localhost:3000 üzerinde çalıştırıldı.
- Metabase'in örnek veritabanı kullanılarak bir dashboard oluşturuldu.

# Web Uygulamasının oluşturulması

- Asp .NET Core MVC şablonuyla bir proje oluşturuldu.
- Dashboard'ın gömüleceği ekran için birer controller ve view oluşturuldu.

# JsonWebToken oluşturulması

1. Dashboard paylaşım yerinden static embedding seçildi ve dashboard publish edildi.
2. JWT için secret key aynı ekrandan alındı.
3. Gerekli paketler kurularak JWT oluşturan bir servis tasarlandı.
4. Kullanıcıyı yetkilendiren bir arayüz oluşturuldu.

# Gömme (Embedding) işleminin tamamlanması

- Dashboard, `<iframe>`kullanılarak sayfaya gömülmüştür.
